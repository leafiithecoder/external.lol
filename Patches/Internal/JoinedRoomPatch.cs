using GorillaNetworking;
using HarmonyLib;

namespace externallol.Patches.Internal
{
    [HarmonyPatch(typeof(PhotonNetworkController), nameof(PhotonNetworkController.OnJoinedRoom))]
    public class JoinedRoomPatch
    {
        public static bool enabled;

        private static void Prefix()
        {
            if (enabled)
                PhotonNetworkController.Instance.currentJoinType = JoinType.FollowingParty;
        }
    }
}
