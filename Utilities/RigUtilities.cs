using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using SharpzReborn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SharpzReborn.Utilities
{
    public class RigUtilities
    {
        public static VRRig GetVRRigFromPlayer(NetPlayer p) =>
            GorillaGameManager.StaticFindRigForPlayer(p);

        public static NetPlayer GetPlayerFromVRRig(VRRig p)
        {
            if (p == null)
                return null;

            if (p.Creator != null)
                return p.Creator;

            return NetworkSystem.Instance.GetPlayer(
                NetworkSystem.Instance.GetOwningPlayerID(p.rigSerializer.gameObject)
            );
        }

        public static NetPlayer GetPlayerFromID(string id) =>
            PhotonNetwork.PlayerList.FirstOrDefault(player => player.UserId == id);

        public static Player NetPlayerToPlayer(NetPlayer p) =>
            p.GetPlayerRef();

        public static Player GetRandomPlayer(bool includeSelf = false) =>
            includeSelf ?
            PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)] :
            PhotonNetwork.PlayerListOthers[Random.Range(0, PhotonNetwork.PlayerListOthers.Length)];

        private static VRRig rigTarget;
        private static float rigTargetChange;
        public static VRRig GetTargetPlayer(float targetChangeDelay = 1f)
        {
            if (!(Time.time > rigTargetChange) && rigTarget.Active()) return rigTarget;
            rigTargetChange = Time.time + targetChangeDelay;
            rigTarget = GetRandomVRRig();

            return rigTarget;
        }

        public static VRRig GetRandomVRRig(bool includeSelf = false) =>
            GetVRRigFromPlayer(GetRandomPlayer(includeSelf));

        public static NetworkView GetNetworkViewFromVRRig(VRRig p) =>
            p.netView;

        public static PhotonView GetPhotonViewFromVRRig(VRRig p) =>
            GetNetworkViewFromVRRig(p).GetView;

        public static VRRig GetClosestVRRig() =>
            VRRig.LocalRig.GetClosest();

        public static readonly Dictionary<string, float> waitingForCreationDate = new Dictionary<string, float>();
        public static readonly Dictionary<string, string> creationDateCache = new Dictionary<string, string>();
        public static string GetCreationDate(string input, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(input, out string date))
                return date;
            if (!waitingForCreationDate.ContainsKey(input))
            {
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }
            else
            {
                if (!(Time.time > waitingForCreationDate[input])) return "Loading...";
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }

            return "Loading...";
        }

        public static void GetCreationCoroutine(string userId, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(userId, out string date))
            {
                onTranslated?.Invoke(date);
                return;
            }

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = userId }, delegate (GetAccountInfoResult result) // Who designed this
            {
                string creationDate = result.AccountInfo.Created.ToString(format);
                creationDateCache[userId] = creationDate;

                onTranslated?.Invoke(creationDate);
            }, delegate { creationDateCache[userId] = "Error"; onTranslated?.Invoke("Error"); });
        }

        public static VRRig GetRigFromHit(RaycastHit hit)
        {
            if (hit.collider == null || NetworkSystem.Instance == null || !PhotonNetwork.InRoom)
            {
                return null;
            }
            VRRig result;
            try
            {
                VRRig componentInParent = hit.collider.GetComponentInParent<VRRig>();
                NetPlayer netPlayer;
                if (componentInParent != null && RigUtilities.TryGetPlayerFromVRRig(componentInParent, out netPlayer))
                {
                    result = componentInParent;
                }
                else
                {
                    VRRig vrrig = null;
                    float num = 0.75f;
                    foreach (VRRig vrrig2 in VRRigCache.ActiveRigs)
                    {
                        if (!(vrrig2 == null) && !vrrig2.isLocal)
                        {
                            float num2 = Vector3.Distance(vrrig2.transform.position, hit.point);
                            if (num2 < num)
                            {
                                num = num2;
                                vrrig = vrrig2;
                            }
                        }
                    }
                    result = ((vrrig != null && RigUtilities.TryGetPlayerFromVRRig(vrrig, out netPlayer)) ? vrrig : null);
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static bool TryGetPlayerFromVRRig(VRRig p, out NetPlayer player)
        {
            player = null;
            if (p == null || NetworkSystem.Instance == null || !PhotonNetwork.InRoom)
            {
                return false;
            }
            try
            {
                if (p.Creator != null)
                {
                    int actorNumber = p.Creator.ActorNumber;
                    foreach (NetPlayer netPlayer in NetworkSystem.Instance.AllNetPlayers)
                    {
                        if (netPlayer != null && netPlayer.ActorNumber == actorNumber)
                        {
                            player = netPlayer;
                            break;
                        }
                    }
                    if (player != null)
                    {
                        return true;
                    }
                }
                if (p.rigSerializer == null || p.rigSerializer.gameObject == null)
                {
                    return false;
                }
                int owningPlayerID = NetworkSystem.Instance.GetOwningPlayerID(p.rigSerializer.gameObject);
                foreach (NetPlayer netPlayer2 in NetworkSystem.Instance.AllNetPlayers)
                {
                    if (netPlayer2.ActorNumber == owningPlayerID)
                    {
                        player = netPlayer2;
                        break;
                    }
                }
            }
            catch
            {
                player = null;
            }
            return player != null;
        }
    }
}