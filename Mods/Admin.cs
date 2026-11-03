using GorillaLocomotion;
using Photon.Realtime;
using Photon.Voice;
using externallol.Classes;
using externallol.Extensions;
using UnityEngine;
using UnityEngine.UIElements;
using static externallol.Menu.Main;
using static externallol.Utilities.RigUtilities;
using exec = externallol.Classes.Network.External;

namespace externallol.Mods
{
    public class Admin // only for admins
    {
        private static float adminEventDelay;
        public static void AdminKickGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("kick", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }
        public static void AdminHideGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("hide", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void AdminHideAll() =>
            exec.ExecuteCommand("hide", ReceiverGroup.Others);

        public static void AdminKickAll() =>
            exec.ExecuteCommand("kick", ReceiverGroup.Others);

        public static void AdminCrashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("crash", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void AdminCrashAll() =>
            exec.ExecuteCommand("crash", ReceiverGroup.Others);
        public static void AdminLagGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (Time.time > adminEventDelay)
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("lag", lockTarget.GetPlayer().ActorNumber, 50);
                        RPCProtection();
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                gunLocked = false;
            }
        }

        public static void AdminLagSpikeAll() =>
            exec.ExecuteCommand("lag", ReceiverGroup.Others, 1000);
        public static void AdminLagAll() =>
            exec.ExecuteCommand("lag", ReceiverGroup.Others, 50);

        public static void AdminLagSpikeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (Time.time > adminEventDelay)
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("lag", lockTarget.GetPlayer().ActorNumber, 1000);
                        RPCProtection();
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                gunLocked = false;
            }
        }

        public static void AdminAnnoyLobby()
        {
            exec.ExecuteCommand("annoy", ReceiverGroup.Others);
        }

        public static void BringAll()
        {
            if (Time.time > adminEventDelay)
            {
                adminEventDelay = Time.time + 0.05f;
                exec.ExecuteCommand("tp", ReceiverGroup.Others, GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward);
            }
        }

        public static void BringGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.05f;
                        exec.ExecuteCommand("tp", GetPlayerFromVRRig(gunTarget).ActorNumber, GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward);
                    }
                }
            }
        }

        public static void BanGun() // this is for the skidders! this is FULLY unpatched
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.05f;
                        exec.ExecuteCommand("ban", GetPlayerFromVRRig(gunTarget).ActorNumber, GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward);
                    }
                }
            }
        }

        public static void AdminAnnoyGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("annoy", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void UnhideAll() =>
            exec.ExecuteCommand("unhide", ReceiverGroup.All);

        public static void CreateExternalObject(
            PrimitiveType shape,
            Color col,
            Vector3? position = null,
            Quaternion? rotation = null,
            Vector3? scale = null)
        {
            position ??= TrueRightHand().position + Vector3.forward * 1.2f;
            rotation ??= Quaternion.identity;
            scale ??= new Vector3(0.3f, 0.3f, 0.3f);

            ExternalObject.Create(
                shape,
                position.Value,
                rotation.Value,
                scale.Value,
                col
            );
        }

        public static void GripCreate(PrimitiveType shape, Color col, Vector3? pos = null, Quaternion? rot = null, Vector3? sca = null)
        {
            if (rightGrab)
            {
                CreateExternalObject(
                    shape,
                    col,
                    TrueRightHand().position,
                    rot.Value,
                    sca.Value
                );
            }
        }

        public static void CreateExternalObjectGun(
        PrimitiveType shape,
        Color col,
        Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? scale = null)
        {
            if (GetGunInput(false))
            {
                var gunData = RenderGun();
                Vector3 pointerPos = gunData.NewPointer.transform.position;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    adminEventDelay = Time.time + 0.05f;
                    position ??= pointerPos;
                    rotation ??= Quaternion.identity;
                    scale ??= new Vector3(0.3f, 0.3f, 0.3f);

                    ExternalObject.Create(
                        shape,
                        position.Value,
                        rotation.Value,
                        scale.Value,
                        col
                    );
                }
            }
        }
    }
}


