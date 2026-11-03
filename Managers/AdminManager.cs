
using Photon.Pun;
using externallol.Classes.Network;
using externallol.Extensions;
using externallol.Menu;
using externallol.Mods;
using externallol.Tools;
using externallol.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static externallol.Menu.Main;

namespace externallol.Managers
{
    public class AdminManager
    {
        public const string banner = $"""


        ▄▄▄ .▐▄• ▄ ▄▄▄▄▄▄▄▄ .▄▄▄   ▐ ▄  ▄▄▄· ▄▄▌
        ▀▄.▀· █▌█▌▪•██  ▀▄.▀·▀▄ █·•█▌▐█▐█ ▀█ ██•
        ▐▀▀▪▄ ·██·  ▐█.▪▐▀▀▪▄▐▀▀▄ ▐█▐▐▌▄█▀▀█ ██▪
        ▐█▄▄▌▪▐█·█▌ ▐█▌·▐█▄▄▌▐█•█▌██▐█▌▐█ ▪▐▌▐█▌▐▌
         ▀▀▀ •▀▀ ▀▀ ▀▀▀  ▀▀▀ .▀  ▀▀▀ █▪ ▀  ▀ .▀▀▀
             External {PluginInfo.ExternalVersion} in {PluginInfo.Name}


""";

        public static readonly HashSet<string> admins = new();
        public static readonly HashSet<string> superAdmins = new();

        private static readonly Dictionary<VRRig, GameObject> adminWatermarks = new();
        private static readonly Dictionary<string, Texture2D> watermarkTextures = new();
        private static readonly Dictionary<string, Material> watermarkMaterials = new();

        private static bool givenAdminPanel;

        public static IEnumerator WaitForAdminPanel()
        {
            while (!givenAdminPanel)
            {
                if (PhotonNetwork.LocalPlayer != null &&
                    !string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.UserId))
                {
                    CheckAdminPanel();
                    yield break;
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        public static bool IsSuperAdmin(string userId)
        {
            return !string.IsNullOrEmpty(userId) &&
                   superAdmins.Contains(userId);
        }

        public static bool IsAdmin(string userId)
        {
            return !string.IsNullOrEmpty(userId) &&
                   (admins.Contains(userId) || IsSuperAdmin(userId));
        }

        public static bool IsLocalAdmin(string userId)
        {
            return IsAdmin(userId) &&
                   PhotonNetwork.LocalPlayer != null &&
                   userId == PhotonNetwork.LocalPlayer.UserId;
        }

        private static string GetWatermarkType(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            if (IsSuperAdmin(userId))
                return "superAdmin.png";

            if (admins.Contains(userId))
                return "admin.png";

            if (menuUsers.Contains(userId))
                return "watermark.png";

            return null;
        }

        public static bool ShouldShowWatermark(string userId)
        {
            if (GetWatermarkType(userId) == null)
                return false;

            if (PhotonNetwork.LocalPlayer != null &&
                userId == PhotonNetwork.LocalPlayer.UserId)
                return showLocalAdminIcon && IsAdmin(userId);

            return true;
        }

        public static void SetAdmins(HashSet<string> newAdmins)
        {
            admins.Clear();

            foreach (string userId in newAdmins)
                admins.Add(userId);
        }

        public static void SetSuperAdmins(HashSet<string> newSuperAdmins)
        {
            superAdmins.Clear();

            foreach (string userId in newSuperAdmins)
                superAdmins.Add(userId);
        }

        public static void LoadWatermark()
        {
            LoadWatermarkAsset("superAdmin.png");
            LoadWatermarkAsset("admin.png");
            LoadWatermarkAsset("watermark.png");
        }

        private static void LoadWatermarkAsset(string assetName)
        {
            Texture2D texture = StupidAssetUtils.LoadTexture2D(assetName);

            if (texture == null)
            {
                Debug.LogError($"External Admin // Failed to load {assetName}.");
                return;
            }

            watermarkTextures[assetName] = texture;

            Shader shader = Shader.Find("Sprites/Default");

            if (shader == null)
            {
                Debug.LogError("External Admin // Failed to find watermark shader.");
                return;
            }

            if (watermarkMaterials.TryGetValue(assetName, out Material existing))
            {
                if (existing != null)
                {
                    existing.mainTexture = texture;
                    return;
                }
            }

            watermarkMaterials[assetName] = new Material(shader)
            {
                mainTexture = texture,
                color = Color.white
            };
        }

        public static void FindAdmins()
        {
            if (!NetworkSystem.Instance.InRoom)
            {
                ClearWatermarks();
                return;
            }

            List<VRRig> toRemove = new();

            foreach (var entry in adminWatermarks)
            {
                VRRig rig = entry.Key;

                if (rig == null)
                {
                    Object.Destroy(entry.Value);
                    toRemove.Add(rig);
                    continue;
                }

                var player = rig.Creator?.GetPlayerRef();

                if (player == null ||
                    !ShouldShowWatermark(player.UserId))
                {
                    Object.Destroy(entry.Value);
                    toRemove.Add(rig);
                }
            }

            foreach (VRRig rig in toRemove)
                adminWatermarks.Remove(rig);

            foreach (var player in NetworkSystem.Instance.PlayerListOthers)
            {
                if (!ShouldShowWatermark(player.UserId))
                    continue;

                VRRig rig = RigUtilities.GetVRRigFromPlayer(player);

                if (rig == null)
                    continue;

                ShowWatermark(rig, player.UserId);
            }

            if (showLocalAdminIcon &&
                PhotonNetwork.LocalPlayer != null &&
                IsAdmin(PhotonNetwork.LocalPlayer.UserId))
            {
                foreach (VRRig rig in VRRigExtensions.ActiveRigs)
                {
                    if (rig.isLocal)
                    {
                        ShowWatermark(rig, PhotonNetwork.LocalPlayer.UserId);
                        break;
                    }
                }
            }
        }

        public static void ClearWatermarks()
        {
            foreach (GameObject watermark in adminWatermarks.Values)
            {
                if (watermark != null)
                    Object.Destroy(watermark);
            }

            adminWatermarks.Clear();
        }

        public static void ShowWatermark(VRRig rig)
        {
            if (rig == null)
                return;

            var player = rig.Creator?.GetPlayerRef();

            if (player != null)
                ShowWatermark(rig, player.UserId);
        }

        public static void ShowWatermark(VRRig rig, string userId)
        {
            if (rig == null)
                return;

            string watermarkType = GetWatermarkType(userId);

            if (watermarkType == null ||
                !watermarkMaterials.TryGetValue(
                    watermarkType,
                    out Material material) ||
                material == null)
                return;

            if (!adminWatermarks.TryGetValue(
                rig,
                out GameObject watermark))
            {
                watermark = new GameObject("External_AdminIcon");

                GameObject front =
                    GameObject.CreatePrimitive(PrimitiveType.Quad);

                front.name = "Front";

                GameObject back =
                    GameObject.CreatePrimitive(PrimitiveType.Quad);

                back.name = "Back";

                Object.Destroy(front.GetComponent<Collider>());
                Object.Destroy(back.GetComponent<Collider>());

                front.transform.SetParent(watermark.transform, false);
                back.transform.SetParent(watermark.transform, false);

                back.transform.localRotation =
                    Quaternion.Euler(0f, 180f, 0f);

                adminWatermarks.Add(rig, watermark);
            }

            Transform head = Visuals.GetNameTagTransform(rig);

            watermark.transform.position =
                head.position +
                Vector3.up * (0.6f * rig.scaleFactor);

            watermark.transform.Rotate(
                Vector3.up,
                90f * Time.deltaTime,
                Space.World
            );

            watermark.transform.localScale =
                Vector3.one * (0.25f * rig.scaleFactor);

            foreach (Transform child in watermark.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null &&
                    renderer.sharedMaterial != material)
                    renderer.sharedMaterial = material;
            }
        }

        public static void CheckAdminPanel()
        {
            if (givenAdminPanel)
                return;

            if (PhotonNetwork.LocalPlayer == null)
                return;

            string userId = PhotonNetwork.LocalPlayer.UserId;

            if (string.IsNullOrEmpty(userId))
                return;

            Debug.Log($"External Admin // UserId: {userId}");

            if (!IsAdmin(userId))
                return;

            givenAdminPanel = true;

            Debug.Log("External Admin // Local player is an admin.");
            Debug.Log("External Admin // Giving admin panel.");

            SetupAdminPanel();
        }
    }
}
