using GorillaGameModes;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Classes;
using SharpzReborn.Extensions;
using SharpzReborn.Menu;
using SharpzReborn.Notifications;
using System.Linq;
using UnityEngine;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Utilities.GameModeUtilities;
using static SharpzReborn.Utilities.RigUtilities;

namespace SharpzReborn.Mods
{
    public class Advantages
    {
        private static bool instantTag = true;
        public static void InstantTagPlayer(NetPlayer Target)
        {
            if (!VRRig.LocalRig.IsTagged() || Target.VRRig().IsTagged())
                return;

            Vector3 archiveRigPosition = VRRig.LocalRig.transform.position;
            VRRig.LocalRig.transform.position = GetVRRigFromPlayer(Target).transform.position;

            SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = new[] { PhotonNetwork.MasterClient.ActorNumber } });
            GameMode.ReportTag(Target);

            VRRig.LocalRig.transform.position = archiveRigPosition;

            RPCProtection();
        }

        private static float tagGunDelay;
        public static void InstantTagGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > tagGunDelay)
                {
                    try
                    {
                        VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                        if (gunTarget && !gunTarget.IsLocal())
                        {
                            tagGunDelay = Time.time + 0.2f;
                            InstantTagPlayer(NetPlayerToPlayer(GetPlayerFromVRRig(gunTarget)));
                        }
                    }
                    catch { }
                }
            }
        }

        public static void TagGun()
        {
            if (!NetworkSystem.Instance.InRoom) return;
            if (instantTag)
            {
                InstantTagGun();
                return;
            }

            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (!lockTarget.IsTagged())
                    {
                        VRRig.LocalRig.enabled = false;

                        if (!Buttons.GetIndex("Obnoxious Tag").enabled)
                            VRRig.LocalRig.transform.position = lockTarget.transform.position - new Vector3(0f, 3f, 0f);
                        else
                        {
                            Vector3 position = lockTarget.transform.position + RandomVector3();

                            VRRig.LocalRig.transform.position = position;

                            VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                            VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();
                            VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();

                            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                            VRRig.LocalRig.leftIndex.calcT = 0f;
                            VRRig.LocalRig.leftMiddle.calcT = 0f;
                            VRRig.LocalRig.leftThumb.calcT = 0f;

                            VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                            VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                            VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                            VRRig.LocalRig.rightIndex.calcT = 0f;
                            VRRig.LocalRig.rightMiddle.calcT = 0f;
                            VRRig.LocalRig.rightThumb.calcT = 0f;

                            VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                            VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                            VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
                        }

                        if (ValidateTag(lockTarget))
                            ReportTag(lockTarget);
                    }
                    else
                    {
                        gunLocked = false;
                        VRRig.LocalRig.enabled = true;
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        if (PhotonNetwork.IsMasterClient)
                            AddInfected(GetPlayerFromVRRig(gunTarget));
                        else
                        {
                            if (!VRRig.LocalRig.IsTagged()) return;
                            gunLocked = true;
                            lockTarget = gunTarget;
                        }
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }
        public static void TagSelf()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                AddInfected(PhotonNetwork.LocalPlayer);
                NotificationManager.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> You have been tagged.");
            }
            else
            {
                if (InfectedList().Contains(PhotonNetwork.LocalPlayer))
                {
                    NotificationManager.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> You have been tagged.");
                    VRRig.LocalRig.enabled = true;
                }
                else
                {
                    VRRig rig = VRRigExtensions.ActiveRigs
                        .Where(r => !r.IsLocal() && r.IsTagged())
                        .OrderBy(r => r.Distance(VRRig.LocalRig)
                                    + r.LatestVelocity().magnitude)
                        .FirstOrDefault();

                        if (!rig.IsTagged()) return;
                        VRRig.LocalRig.enabled = false;
                        if (rig != null) VRRig.LocalRig.transform.position = rig.rightHandTransform.position;

                }
            }
        }

        public static void UntagAll()
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                NotificationManager.SendNotification($"{fail} You are not master client.");
            else
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    RemoveInfected(v);
            }
        }
        public static int vbux;
        public static void DisableTags()
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                NotificationManager.SendNotification($"{fail} You are not master client.");
            else
            {
                if (vbux == 2)
                {
                    foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                        RemoveInfected(v);
                }
            }
            NotificationManager.SendNotification($"{warning} You should Flush RPCs after this.");
            vbux++;
        }

        public static bool ValidateTag(VRRig Rig) =>
            Vector3.Distance(ServerSyncPos, Rig.transform.position) < 6f;

        private static float reportTagDelay;
        public static void ReportTag(VRRig rig)
        {
            if (Time.time > reportTagDelay)
            {
                reportTagDelay = Time.time + 0.1f;
                GameMode.ReportTag(rig.GetPlayer());
            }
        }
        public static void TagAll()
        {
            if (!NetworkSystem.Instance.IsMasterClient) NotificationManager.SendNotification($"{fail} You are not the master client.");
            if (!NetworkSystem.Instance.InRoom) return;

            static void TurnOff()
            {
                Buttons.GetIndex("Tag All").SetEnabled(false);
                RecreateMenu();
            }


            if (NetworkSystem.Instance.IsMasterClient)
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    AddInfected(v);

                TurnOff();
                NotificationManager.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> Everyone is tagged!");
            }
        }

        public static void UntagGun()
        {
            if (!GetGunInput(false))
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                    VRRig.LocalRig.enabled = true;
                }

                return;
            }

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;

            if (gunLocked && lockTarget != null)
            {
                if (lockTarget.IsTagged())
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        NotificationManager.SendNotification($"{fail} You are not the master client.");
                        gunLocked = false;
                        lockTarget = null;
                        return;
                    }

                    RemoveInfected(RigManager.GetPlayerFromVRRig(lockTarget));

                    gunLocked = false;
                    lockTarget = null;
                }
                else
                {
                    gunLocked = false;
                    lockTarget = null;
                    VRRig.LocalRig.enabled = true;
                }

                return;
            }

            if (GetGunInput(true) && Ray.collider != null)
            {
                VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

                if (gunTarget && !gunTarget.IsLocal() && gunTarget.IsTagged())
                {
                    gunLocked = true;
                    lockTarget = gunTarget;
                }
            }
        }

        public static void FlickTagGun()
        {
            if (!GetGunInput(false))
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                }

                return;
            }

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;
            GameObject NewPointer = GunData.NewPointer;

            if (gunLocked && lockTarget != null)
            {
                Transform controller = GTPlayer.Instance.GetControllerTransform(false);

                Vector3 bodyPosition = GorillaTagger.Instance.bodyCollider.transform.position;
                Vector3 targetPosition = lockTarget.transform.position;

                Vector3 offset = targetPosition - bodyPosition;

                if (offset.sqrMagnitude > 16f)
                    targetPosition = bodyPosition + offset.normalized * 4f;

                controller.position = targetPosition;

                return;
            }

            if (GetGunInput(true) && Ray.collider != null)
            {
                VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

                if (gunTarget && !gunTarget.IsLocal())
                {
                    gunLocked = true;
                    lockTarget = gunTarget;
                }
            }
        }
    }
}
