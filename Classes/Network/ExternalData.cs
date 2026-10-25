using SharpzReborn.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.Newtonsoft.Json.Linq;

namespace SharpzReborn.Classes.Network
{
    public static class ExternalData
    {
        private const string AdminDataUrl =
            "https://api.github.com/repos/leafiithecoder/SharpzData/contents/admins.json?ref=main";

        public static bool Loaded { get; private set; }

        public static void Load()
        {
            Loaded = false;
            CoroutineManager.Instance.StartCoroutine(LoadAdmins());
        }

        private static IEnumerator LoadAdmins()
        {
            using UnityWebRequest request =
                UnityWebRequest.Get(AdminDataUrl);

            request.SetRequestHeader(
                "Accept",
                "application/vnd.github.raw+json"
            );

            request.SetRequestHeader(
                "X-GitHub-Api-Version",
                "2022-11-28"
            );

            request.SetRequestHeader(
                "User-Agent",
                "SharpzReborn"
            );

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"External // Failed to load admin data: {request.error}"
                );

                yield break;
            }

            try
            {
                JObject json =
                    JObject.Parse(request.downloadHandler.text);

                JArray admins =
                    json["admins"] as JArray;

                if (admins == null)
                {
                    Debug.LogError(
                        "External // Admin data does not contain an admins array."
                    );

                    yield break;
                }

                HashSet<string> adminIds = new();
                HashSet<string> superAdminIds = new();

                foreach (JToken admin in admins)
                {
                    string userId =
                        admin["userId"]?.ToString();

                    if (string.IsNullOrEmpty(userId))
                        continue;

                    adminIds.Add(userId);

                    bool isSuperAdmin =
                        (bool?)admin["superAdmin"] ?? false;

                    if (isSuperAdmin)
                        superAdminIds.Add(userId);
                }

                AdminManager.SetAdmins(adminIds);
                AdminManager.SetSuperAdmins(superAdminIds);

                Loaded = true;

                Debug.Log(
                    $"External // Loaded {adminIds.Count} admins, " +
                    $"{superAdminIds.Count} superadmins."
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    $"External // Failed to parse admin data: {ex}"
                );
            }
        }
    }
}