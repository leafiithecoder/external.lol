using Photon.Voice;
using externallol.Mods;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace externallol.Managers
{
    public class VoiceManager : IAudioReader<float>
    {
        private int samplingRate = 48000;
        private int outputRate = 48000;
        private float gain = 1f;
        private float clipVolume = 1f;
        private float pitch = 1f;
        private float clipSpeed = 1f;

        private string currentDevice;
        public AudioClip microphoneClip;
        private int lastSamplePosition;
        private float step;

        private string error;

        private float[] rawMicrophoneData;
        private float[] microphoneBuffer;
        private float resamplePointer;

        private readonly object audioClipsLock = new object();

        public sealed class Clip
        {
            public Guid Id { get; set; }
            public AudioClip Source { get; set; }
            public float[] Samples;
            public int Channels;
            private float _samplePosition;

            public float Step { get; set; }
            public bool MuteMicrophone { get; set; }
            public float Volume { get; set; } = 1f;
            public float Speed { get; set; } = 1f;
            public bool IsPaused { get; set; }

            public float Length => Samples != null && Channels > 0
                ? (float)(Samples.Length / Channels) / Instance.OutputRate
                : 0f;

            public float CurrentTime
            {
                get => Samples != null && Channels > 0
                    ? _samplePosition / (Samples.Length / Channels) * Length
                    : 0f;
                set => Seek(value);
            }

            public float InternalPosition
            {
                get => _samplePosition;
                set => _samplePosition = value;
            }

            public void Pause() => IsPaused = true;

            public void Resume() => IsPaused = false;

            public void Seek(float seconds)
            {
                if (Samples == null || Channels <= 0) return;

                float targetSample = seconds * Instance.OutputRate;
                int maxFrames = Samples.Length / Channels;

                _samplePosition = Mathf.Clamp(targetSample, 0, maxFrames);
            }
        }

        private readonly List<Clip> audioClips = new List<Clip>();

        private bool muteMicrophone;

        public VoiceManager(string device = null)
        {
            Instance ??= this;
            StartRecording(device);
        }

        public IReadOnlyList<Clip> AudioClips
        {
            get
            {
                lock (audioClipsLock)
                    return audioClips.ToArray();
            }
        }

        public bool MuteMicrophone
        {
            get => muteMicrophone;
            set => muteMicrophone = value;
        }

        public int SamplingRate
        {
            get => samplingRate;
            set
            {
                samplingRate = Mathf.Max(8000, value);
                RestartMicrophone();
            }
        }

        public int OutputRate
        {
            get => outputRate;
            set
            {
                outputRate = Mathf.Max(8000, value);
                RestartMicrophone();
            }
        }

        public float Gain
        {
            get => gain;
            set => gain = Mathf.Max(0f, value);
        }

        public float ClipVolume
        {
            get => clipVolume;
            set => clipVolume = Mathf.Max(0f, value);
        }

        public float Pitch
        {
            get => pitch;
            set => pitch = Mathf.Max(0.1f, value);
        }

        public float ClipSpeed
        {
            get => clipSpeed;
            set => clipSpeed = Mathf.Max(0.1f, value);
        }

        public readonly Dictionary<string, Action<float[]>> PostProcessors =
            new Dictionary<string, Action<float[]>>();

        public bool PostProcessClip { get; set; }

        public int Channels => 2;
        public string Error => error;
        public string CurrentDevice => currentDevice;

        public static VoiceManager Instance { get; private set; }

        public static VoiceManager Get(string device = null)
        {
            return Instance ??= new VoiceManager(device);
        }

        public bool StartRecording(string device = null)
        {
            error = null;

            if (Microphone.devices == null || Microphone.devices.Length == 0)
            {
                error = "No microphone devices found";
                Debug.LogWarning(error);
                return false;
            }

            if (string.IsNullOrEmpty(device))
                currentDevice = Microphone.devices[0];
            else
            {
                bool found = false;

                for (int i = 0; i < Microphone.devices.Length; i++)
                {
                    if (Microphone.devices[i] == device)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    error = $"Microphone device '{device}' not found";
                    Debug.LogError(error);
                    return false;
                }

                currentDevice = device;
            }

            if (Microphone.IsRecording(currentDevice))
                Microphone.End(currentDevice);

            microphoneClip = Microphone.Start(
                currentDevice,
                false,
                60,
                samplingRate
            );

            if (microphoneClip == null)
            {
                error = $"Failed to start microphone '{currentDevice}'";
                Debug.LogError(error);
                return false;
            }

            lastSamplePosition = 0;
            step = samplingRate / (float)OutputRate;
            resamplePointer = 0f;

            return true;
        }

        public bool StopRecording()
        {
            if (!string.IsNullOrEmpty(currentDevice) &&
                Microphone.IsRecording(currentDevice))
                Microphone.End(currentDevice);

            microphoneClip = null;
            lastSamplePosition = 0;
            resamplePointer = 0f;

            return true;
        }

        public bool SwitchMicrophone(string device)
            => StopRecording() && StartRecording(device);

        public bool RestartMicrophone()
            => StopRecording() && StartRecording(currentDevice);

        public Clip AudioClip(AudioClip audioClip, bool disableMicrophone = false)
        {
            if (audioClip == null)
                return null;

            Guid id = Guid.NewGuid();
            int channels = Mathf.Max(1, audioClip.channels);

            float[] raw = new float[audioClip.samples * channels];
            audioClip.GetData(raw, 0);

            try
            {
                if (audioClip.frequency != OutputRate)
                    raw = Resample(raw, audioClip.frequency, OutputRate, channels);

                Clip clipState = new Clip
                {
                    Id = id,
                    Source = audioClip,
                    Samples = raw,
                    Channels = channels,
                    Step = 1f,
                    MuteMicrophone = disableMicrophone,
                    Volume = clipVolume,
                    Speed = clipSpeed
                };

                lock (audioClipsLock)
                    audioClips.Add(clipState);

                return clipState;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to insert audio clip: {e}");
                return null;
            }
        }

        public Clip GetAudioClip(Guid id)
        {
            lock (audioClipsLock)
            {
                int index = audioClips.FindIndex(c => c.Id == id);

                if (index == -1)
                    return null;

                return audioClips[index];
            }
        }

        public static float[] Resample(
            float[] source,
            int sourceRate,
            int targetRate,
            int channels)
        {
            if (source == null || source.Length == 0 ||
                sourceRate <= 0 || sourceRate == targetRate)
                return source;

            int sourceSamples = Mathf.Max(1, source.Length / channels);
            float lengthInSeconds = (float)sourceSamples / sourceRate;
            int targetSamples = Mathf.Max(
                1,
                Mathf.RoundToInt(lengthInSeconds * targetRate)
            );

            float[] target = new float[targetSamples * channels];

            if (sourceSamples == 1 || targetSamples == 1)
            {
                for (int c = 0; c < channels && c < target.Length; c++)
                    target[c] = source[Mathf.Clamp(c, 0, source.Length - 1)];
            }
            else
            {
                float ratio = (sourceSamples - 1f) / (targetSamples - 1f);

                for (int i = 0; i < targetSamples; i++)
                {
                    float p = i * ratio;
                    int a = Mathf.Clamp((int)p, 0, sourceSamples - 1);
                    int b = Mathf.Clamp(a + 1, 0, sourceSamples - 1);
                    float t = p - a;

                    for (int c = 0; c < channels; c++)
                    {
                        int o = i * channels + c;
                        int ia = Mathf.Clamp(a * channels + c, 0, source.Length - 1);
                        int ib = Mathf.Clamp(b * channels + c, 0, source.Length - 1);

                        target[o] = Mathf.Lerp(source[ia], source[ib], t);
                    }
                }
            }

            return target;
        }

        public bool StopAudioClip(Clip clip)
        {
            if (clip == null) return false;

            lock (audioClipsLock)
                return audioClips.Remove(clip);
        }

        public void StopAudioClips()
        {
            lock (audioClipsLock)
                audioClips.Clear();
        }


        public bool Read(float[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return false;

            if (microphoneBuffer == null || microphoneBuffer.Length != buffer.Length)
                microphoneBuffer = new float[buffer.Length];

            Array.Clear(microphoneBuffer, 0, microphoneBuffer.Length);

            if (microphoneClip != null && !string.IsNullOrEmpty(currentDevice))
            {
                int outFrames = buffer.Length / Channels;
                int micChannels = Mathf.Max(1, microphoneClip.channels);
                int micFrames = microphoneClip.samples;
                int micSampleCount = micFrames * micChannels;

                if (rawMicrophoneData == null || rawMicrophoneData.Length != micSampleCount)
                    rawMicrophoneData = new float[micSampleCount];

                int curFrame = Microphone.GetPosition(currentDevice);
                int available = curFrame - lastSamplePosition;

                if (available >= 0)
                {
                    float sourceStep = microphoneClip.frequency / (float)outputRate * pitch;
                    int needed = Mathf.CeilToInt(outFrames * sourceStep) + 2;

                    if (available >= needed)
                    {
                        microphoneClip.GetData(rawMicrophoneData, 0);

                        bool muteMicForClip = false;

                        lock (audioClipsLock)
                        {
                            for (int i = 0; i < audioClips.Count; i++)
                            {
                                if (!audioClips[i].IsPaused && audioClips[i].MuteMicrophone)
                                {
                                    muteMicForClip = true;
                                    break;
                                }
                            }
                        }

                        float sourcePosition = lastSamplePosition + resamplePointer;

                        for (int i = 0; i < buffer.Length; i += Channels)
                        {
                            float left = 0f;
                            float right = 0f;

                            if (!muteMicrophone && !muteMicForClip)
                            {
                                int aFrame = Mathf.Clamp((int)sourcePosition, 0, micFrames - 1);
                                int bFrame = Mathf.Min(aFrame + 1, micFrames - 1);

                                float frac = sourcePosition - Mathf.Floor(sourcePosition);

                                if (micChannels == 1)
                                {
                                    float a = rawMicrophoneData[aFrame];
                                    float b = rawMicrophoneData[bFrame];

                                    left = right = Mathf.Lerp(a, b, frac) * gain;
                                }
                                else
                                {
                                    int a = aFrame * micChannels;
                                    int b = bFrame * micChannels;

                                    left = Mathf.Lerp(rawMicrophoneData[a], rawMicrophoneData[b], frac) * gain;
                                    right = Mathf.Lerp(rawMicrophoneData[a + 1], rawMicrophoneData[b + 1], frac) * gain;
                                }
                            }

                            microphoneBuffer[i] = left;

                            if (i + 1 < buffer.Length)
                                microphoneBuffer[i + 1] = right;

                            sourcePosition += sourceStep;
                        }

                        int usedFrames = Mathf.FloorToInt(sourcePosition) - lastSamplePosition;

                        lastSamplePosition += usedFrames;
                        resamplePointer = sourcePosition - Mathf.Floor(sourcePosition);
                    }
                }
            }

            if (!PostProcessClip)
            {
                foreach (var postProcess in PostProcessors.Values)
                    postProcess?.Invoke(microphoneBuffer);
            }

            Array.Copy(microphoneBuffer, buffer, buffer.Length);

            lock (audioClipsLock)
            {
                for (int c = audioClips.Count - 1; c >= 0; c--)
                {
                    var clip = audioClips[c];

                    if (clip.IsPaused)
                        continue;

                    bool clipFinished = false;

                    for (int i = 0; i < buffer.Length; i += Channels)
                    {
                        int index = (int)clip.InternalPosition;
                        int maxFrames = clip.Samples.Length / clip.Channels;

                        if (index >= maxFrames)
                        {
                            clipFinished = true;
                            break;
                        }

                        int nextIndex = Mathf.Min(index + 1, maxFrames - 1);
                        float frac = clip.InternalPosition - index;

                        float left;
                        float right;

                        if (clip.Channels == 1)
                        {
                            left = right = Mathf.Lerp(
                                clip.Samples[index],
                                clip.Samples[nextIndex],
                                frac
                            ) * clip.Volume;
                        }
                        else
                        {
                            int baseIdx1 = index * clip.Channels;
                            int baseIdx2 = nextIndex * clip.Channels;

                            left = Mathf.Lerp(
                                clip.Samples[baseIdx1],
                                clip.Samples[baseIdx2],
                                frac
                            ) * clip.Volume;

                            right = Mathf.Lerp(
                                clip.Samples[baseIdx1 + 1],
                                clip.Samples[baseIdx2 + 1],
                                frac
                            ) * clip.Volume;
                        }

                        buffer[i] += left;

                        if (i + 1 < buffer.Length)
                            buffer[i + 1] += right;

                        clip.InternalPosition += Mathf.Max(0.0001f, clip.Step * clip.Speed);

                        if (clip.InternalPosition >= maxFrames)
                        {
                            clipFinished = true;
                            break;
                        }
                    }

                    if (clipFinished)
                        audioClips.RemoveAt(c);
                }
            }

            if (PostProcessClip)
            {
                foreach (var postProcess in PostProcessors.Values)
                    postProcess?.Invoke(buffer);
            }

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Mathf.Clamp(buffer[i], -1f, 1f);

            return true;
        }


        public void Dispose()
        {
            StopRecording();
            StopAudioClips();

            if (ReferenceEquals(Instance, this))
                Instance = null;
        }
    }
}