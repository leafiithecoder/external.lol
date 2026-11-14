using GorillaLocomotion;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using static externallol.Menu.Main;

namespace externallol.Mods
{
    public class Player
    {
        public static void GrabRig()
        {
            if (rightGrab)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }
            else
            {
                VRRig.LocalRig.enabled = true;
            }
        }

        private static bool ghostMonke;
        private static bool lastHit;

        public static void Ghost()
        {
            bool hit = rightPrimary || Mouse.current.leftButton.isPressed;

            VRRig.LocalRig.enabled = !ghostMonke;

            if (hit && !lastHit)
                ghostMonke = !ghostMonke;

            lastHit = hit;
        }

        private static bool wasDisabledAlready;
        private static bool invisMonke;
        private static bool lastHit2;

        public static void Invisible()
        {
            bool hit = rightSecondary || Mouse.current.rightButton.isPressed;

            if (invisMonke)
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position - Vector3.up * 99999f;
            }

            if (hit && !lastHit2)
            {
                invisMonke = !invisMonke;

                if (invisMonke)
                    wasDisabledAlready = VRRig.LocalRig.enabled;
                else
                    VRRig.LocalRig.enabled = wasDisabledAlready;
            }

            lastHit2 = hit;
        }

        public static GameObject gsphereR;
        public static GameObject gsphereL;

        public static void GhostView()
        {
            if (!ghostMonke && !invisMonke)
            {
                if (gsphereL != null)
                {
                    UnityEngine.Object.Destroy(gsphereL);
                    gsphereL = null;
                }

                if (gsphereR != null)
                {
                    UnityEngine.Object.Destroy(gsphereR);
                    gsphereR = null;
                }

                return;
            }

            if (gsphereL == null)
            {
                gsphereL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gsphereL.transform.localScale = Vector3.one * 0.111f;
                gsphereL.GetComponent<Renderer>().material.color = Color.rebeccaPurple;
                gsphereL.GetComponent<Collider>().enabled = false;
            }

            if (gsphereR == null)
            {
                gsphereR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gsphereR.transform.localScale = Vector3.one * 0.111f;
                gsphereR.GetComponent<Renderer>().material.color = Color.rebeccaPurple;
                gsphereR.GetComponent<Collider>().enabled = false;
            }

            gsphereL.transform.position =
                TrueLeftHand().position;

            gsphereR.transform.position =
                TrueRightHand().position;
        }

        private static readonly float headSpinSpeed = 10f;
        public static void SpinHead(string axis)
        {
            if (VRRig.LocalRig.enabled)
            {
                switch (axis.ToLower())
                {
                    case "x":
                        VRRig.LocalRig.head.trackingRotationOffset.x += headSpinSpeed;
                        break;
                    case "y":
                        VRRig.LocalRig.head.trackingRotationOffset.y += headSpinSpeed;
                        break;
                    case "z":
                        VRRig.LocalRig.head.trackingRotationOffset.z += headSpinSpeed;
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (axis.ToLower())
                {
                    case "x":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(headSpinSpeed, 0f, 0f));
                        break;
                    case "y":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, headSpinSpeed, 0f));
                        break;
                    case "z":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, 0f, headSpinSpeed));
                        break;
                    default:
                        return;
                }
            }
        }

        public static void SpazHead(string axis)
        {
            int offset = UnityEngine.Random.Range(0, 360);
            switch (axis.ToLower())
            {
                case "x":
                    VRRig.LocalRig.head.trackingRotationOffset.x = offset;
                    break;
                case "y":
                    VRRig.LocalRig.head.trackingRotationOffset.y = offset;
                    break;
                case "z":
                    VRRig.LocalRig.head.trackingRotationOffset.z = offset;
                    break;
                default:
                    return;
            }
        }
        public static void SpazHeadXYZ()
        {
            if (VRRig.LocalRig.enabled)
            {
                VRRig.LocalRig.head.trackingRotationOffset.x = UnityEngine.Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.y = UnityEngine.Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.z = UnityEngine.Random.Range(0f, 360f);
            }
            else
                VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
        }

        public static void FixHead()
        {
            VRRig.LocalRig.head.trackingRotationOffset.x = 0f;
            VRRig.LocalRig.head.trackingRotationOffset.y = 0f;
            VRRig.LocalRig.head.trackingRotationOffset.z = 0f;
            VRRig.LocalRig.head.rigTarget.transform.localScale = Vector3.one;
        }

        public static void UpsideDownHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.z = 180f;

        public static void BrokenNeck() =>
            VRRig.LocalRig.head.trackingRotationOffset.z = 90f;

        public static void BackwardsHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.y = 180f;

        public static void SidewaysHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.y = 90f;

        public static float lastBangTime;
        public static void HeadBang()
        {
            if (Time.time > lastBangTime)
            {
                VRRig.LocalRig.head.trackingRotationOffset.x = 50f;
                lastBangTime = Time.time + 60f / 159f;
            }
            else
                VRRig.LocalRig.head.trackingRotationOffset.x = Mathf.Lerp(VRRig.LocalRig.head.trackingRotationOffset.x, 0f, 0.1f);
        }

        public static Vector3 offsetLH = Vector3.zero;
        public static Vector3 offsetRH = Vector3.zero;
        public static Vector3 offsetH = Vector3.zero;
        public static void EnableSpazRig()
        {
            ghostException = true;
            offsetLH = VRRig.LocalRig.leftHand.trackingPositionOffset;
            offsetRH = VRRig.LocalRig.rightHand.trackingPositionOffset;
            offsetH = VRRig.LocalRig.head.trackingPositionOffset;
        }

        public static void SpazRig()
        {
            if (rightPrimary)
            {
                float spazAmount = 0.1f;
                ghostException = true;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH + RandomVector3(spazAmount);
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH + RandomVector3(spazAmount);
                VRRig.LocalRig.head.trackingPositionOffset = offsetH + RandomVector3(spazAmount);
            }
            else
            {
                ghostException = false;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH;
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH;
                VRRig.LocalRig.head.trackingPositionOffset = offsetH;
            }
        }

        public static void DisableSpazRig()
        {
            ghostException = false;
            VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH;
            VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH;
            VRRig.LocalRig.head.trackingPositionOffset = offsetH;
        }

        public static void SpazHands()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.leftHandTransform.position + VRRig.LocalRig.leftHand.rigTarget.transform.forward * 3f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.position + VRRig.LocalRig.rightHand.rigTarget.transform.forward * 3f;
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void SpazRealHands()
        {
            if (rightPrimary)
            {
                GTPlayer.Instance.GetControllerTransform(true).rotation = RandomQuaternion();
                GTPlayer.Instance.GetControllerTransform(true).position = GorillaTagger.Instance.leftHandTransform.position + GTPlayer.Instance.GetControllerTransform(true).forward * 3f;

                GTPlayer.Instance.GetControllerTransform(false).rotation = RandomQuaternion();
                GTPlayer.Instance.GetControllerTransform(false).position = GorillaTagger.Instance.rightHandTransform.position + GTPlayer.Instance.GetControllerTransform(false).forward * 3f;
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void FixRigHandRotation()
        {
            VRRig.LocalRig.leftHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.leftHand.trackingRotationOffset);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.rightHand.trackingRotationOffset);
        }

        public static void AutoTPose()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void RigGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true))
                {
                    VRRig.LocalRig.enabled = false;
                    VRRig.LocalRig.transform.position = NewPointer.transform.position + new Vector3(0, 1, 0);
                }
                else
                    VRRig.LocalRig.enabled = true;
            }
        }

        public static void Helicopter()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position += new Vector3(0f, 0.05f, 0f);
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void Fan()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + (VRRig.LocalRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + VRRig.LocalRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position - (VRRig.LocalRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + VRRig.LocalRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void MinecraftAnimations()
        {
            VRRig.LocalRig.enabled = false;

            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            if (rightPrimary)
            {
                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f + GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin(Time.frameCount / 10f);
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f + -(GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin(Time.frameCount / 10f));
            }
            else
            {
                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f;
            }

            if (rightSecondary)
            {
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + Vector3.Lerp(GorillaTagger.Instance.rightHandTransform.forward, -GorillaTagger.Instance.rightHandTransform.up, 0.5f) * 2f;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }

            FixRigHandRotation();
        }
    }
}
