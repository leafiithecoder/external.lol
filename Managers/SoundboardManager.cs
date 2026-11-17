
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GorillaNetworking;
using externallol.Classes;
using externallol.Menu;
using UnityEngine;
using UnityEngine.Networking;

namespace externallol.Managers
{
    public static class SoundboardManager
    {
        public static readonly Dictionary<string, AudioClip> AudioFilePool =
            new(StringComparer.OrdinalIgnoreCase);

        private static AudioSource audioSource;
        private static VoiceManager.Clip voiceClip;
        private static ButtonInfo currentButton;
        private static bool isLoading;

        public static void LoadAllSounds()
        {
            ReloadSounds();
        }

        public static void ReloadSounds()
        {
            if (isLoading)
                return;

            FolderManager.Initialize();
            StopCurrentSound();

            foreach (AudioClip clip in AudioFilePool.Values)
                UnityEngine.Object.Destroy(clip);

            AudioFilePool.Clear();

            isLoading = true;

            Buttons.GetCategory("Soundboard").buttons = Buttons.Soundboard;
            RefreshMenu();

            CoroutineManager.Instance.StartCoroutine(LoadSounds());
        }

        private static IEnumerator LoadSounds()
        {
            string[] files = Directory.GetFiles(FolderManager.Sounds);

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file).ToLowerInvariant();

                AudioType type;

                switch (extension)
                {
                    case ".wav":
                        type = AudioType.WAV;
                        break;

                    case ".ogg":
                        type = AudioType.OGGVORBIS;
                        break;

                    case ".mp3":
                        type = AudioType.MPEG;
                        break;

                    default:
                        continue;
                }

                using UnityWebRequest request =
                    UnityWebRequestMultimedia.GetAudioClip(
                        new Uri(file).AbsoluteUri,
                        type
                    );

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning(
                        $"externallol // Failed to load sound {Path.GetFileName(file)}: {request.error}"
                    );

                    continue;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

                if (clip != null)
                    AudioFilePool[file] = clip;
            }

            BuildSoundButtons();

            isLoading = false;
            RefreshMenu();

            Debug.Log($"externallol // Loaded {AudioFilePool.Count} sounds.");
        }

        private static void BuildSoundButtons()
        {
            List<ButtonInfo> buttons = Buttons.Soundboard.ToList();

            foreach (var entry in AudioFilePool)
            {
                string file = entry.Key;
                string soundName = Path.GetFileNameWithoutExtension(file);

                ButtonInfo button = new()
                {
                    buttonText = soundName,
                    mode = ButtonMode.Action,
                    toolTip = $"Plays {soundName}.",
                };

                button.method = () => PlaySound(file, button);

                buttons.Add(button);
            }

            Buttons.GetCategory("Soundboard").buttons = buttons.ToArray();
        }

        private static void PlaySound(string file, ButtonInfo button)
        {
            if (!AudioFilePool.TryGetValue(file, out AudioClip clip))
                return;

            StopCurrentSound();

            if (audioSource == null)
            {
                GameObject audioObject = new("ExternalSoundboardAudio");
                audioSource = audioObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 0f;
                audioSource.playOnAwake = false;
            }

            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.volume = 1f;
            audioSource.loop = false;
            audioSource.Play();

            currentButton = button;
            currentButton.enabled = true;

            if (NetworkSystem.Instance.InRoom)
                voiceClip = VoiceManager.Get().AudioClip(clip);

            CoroutineManager.Instance.StartCoroutine(MonitorPlayback(button));

            RefreshMenu();
        }

        private static IEnumerator MonitorPlayback(ButtonInfo button)
        {
            while (currentButton == button &&
                   audioSource != null &&
                   audioSource.isPlaying)
            {
                yield return null;
            }

            if (currentButton != button)
                yield break;

            button.enabled = false;
            currentButton = null;

            RefreshMenu();
        }

        public static void StopCurrentSound()
        {
            if (audioSource != null)
                audioSource.Stop();

            if (voiceClip != null)
                VoiceManager.Get().StopAudioClip(voiceClip);

            voiceClip = null;

            if (currentButton != null)
                currentButton.enabled = false;

            currentButton = null;

            RefreshMenu();
        }

        private static void RefreshMenu()
        {
            if (Main.CurrentCategory?.name == "Soundboard")
                Main.RecreateMenu();
        }
    }
}
