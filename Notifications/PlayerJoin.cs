using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using externallol.Managers;
using externallol.Notifications;
using UnityEngine;

namespace externallol.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    public class JoinPatch : MonoBehaviour
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                if (AdminManager.IsAdmin(newPlayer.UserId))
                {NotificationManager.SendNotification("<color=grey>[</color><color=green>ADMIN JOIN</color><color=grey>] </color><color=white>Name: " + newPlayer.NickName + "</color>");}
                NotificationManager.SendNotification("<color=grey>[</color><color=green>JOIN</color><color=grey>] </color><color=white>Name: " + newPlayer.NickName + "</color>");oldnewplayer = newPlayer;
            }
        }

        private static Player oldnewplayer;
    }
}