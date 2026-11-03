using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using externallol.Managers;
using externallol.Notifications;
using UnityEngine;

namespace externallol.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    public class LeavePatch : MonoBehaviour
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                if (AdminManager.IsAdmin(otherPlayer.UserId))
                {
                    NotificationManager.SendNotification("<color=grey>[</color><color=red>ADMIN LEAVE</color><color=grey>] </color><color=white>Name: " + otherPlayer.NickName + "</color>");
                }
                    NotificationManager.SendNotification("<color=grey>[</color><color=red>LEAVE</color><color=grey>]</color> <color=white>Name: " + otherPlayer.NickName + "</color>");
                    a = otherPlayer;
            }
        }

        private static Player a;
    }
}