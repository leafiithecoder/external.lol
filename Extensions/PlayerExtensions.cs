using ExitGames.Client.Photon;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using externallol.Utilities;
using System.Linq;
using UnityEngine;

namespace externallol.Extensions
{
    public static class PlayerExtensions
    {
        #region NetPlayer
        public static Player GetPlayer(this NetPlayer self) =>
            RigUtilities.NetPlayerToPlayer(self);

        public static VRRig VRRig(this NetPlayer self) =>
            RigUtilities.GetVRRigFromPlayer(self);

        public static bool InRoom(this NetPlayer self) =>
            NetworkSystem.Instance.AllNetPlayers.Contains(self);

        public static Hashtable GetCustomProperties(this NetPlayer self) =>
            self.GetPlayer().CustomProperties;
        #endregion

        #region Player
        public static VRRig VRRig(this Player self) =>
            RigUtilities.GetVRRigFromPlayer(self);

        public static bool InRoom(this Player self) =>
            PhotonNetwork.PlayerList.Contains(self);

        #endregion

        #region GorillaTagger
        public static bool IsGrounded(this GorillaTagger tagger, float maxDistance = 0.15f) =>
                Physics.Raycast(tagger.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, maxDistance, GTPlayer.Instance.locomotionEnabledLayers);
        #endregion
    }
}
