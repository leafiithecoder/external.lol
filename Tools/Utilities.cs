using GorillaGameModes;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Classes;
using SharpzReborn.Classes.Network;
using SharpzReborn.Managers;
using SharpzReborn.Menu;
using TMPro;
using UnityEngine;

namespace SharpzReborn.Tools
{
    public class Utilities
    {
        public static string GetCurrentGamemode()
        {
            switch (GorillaGameManager.instance.GameType())
            {
                case GorillaGameModes.GameModeType.HuntDown:
                    return "Hunt";

                case GorillaGameModes.GameModeType.FreezeTag:
                    return "Freeze Tag";

                case GorillaGameModes.GameModeType.Ghost:
                    return "Ghost Tag";

                case GorillaGameModes.GameModeType.PropHunt:
                    return "Prop Hunt";

                case GorillaGameModes.GameModeType.InfectionCompetitive:
                    return "Competitive";

                case GorillaGameModes.GameModeType.SuperInfect:
                    return "Super Infection";

                case GorillaGameModes.GameModeType.SuperCasual:
                    return "Super Casual";

                case GorillaGameModes.GameModeType.None:
                    return "ERROR";

                default:
                    return GorillaGameManager.instance.GameType().ToString();
            }
        }

        public static void BroadcastRoom(string roomName, bool create, string key, string shuffler)
        {
            string text = NetworkSystem.ShuffleRoomName(roomName, shuffler.Substring(2, 8), true) + "|" + NetworkSystem.ShuffleRoomName("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(NetworkSystem.Instance.currentRegionIndex, 1), shuffler[..2], true);

            BroadcastMyRoomRequest broadcastMyRoomRequest = new BroadcastMyRoomRequest
            {
                KeyToFollow = key,
                RoomToJoin = text,
                Set = create
            };

            GorillaServer.Instance.BroadcastMyRoom(broadcastMyRoomRequest, delegate { }, delegate { });
        }

        public static string Map(VRRig r)
        {
            return EnumUtilExt.GetName<GTZone>(r.zoneEntity.currentZone);
        }

        public static void Init()
        {
            CoroutineManager.Instance.StartCoroutine(AdminManager.WaitForAdminPanel());
            ExternalData.Load();
            External.Initialize();
            AdminManager.LoadWatermark();
            Boards.SetLaunchBoards();
            Debug.Log($"SharpzReborn // Initialized {Buttons.ButtonCount} buttons.");

            Debug.Log(AdminManager.banner);

        }

        public static void Shutdown()
        {
            External.Shutdown();
            Preferences.Save();
        }

        public static void OnJoin()
        {
            Mods.Room.lastRoom = PhotonNetwork.CurrentRoom.Name;
            RoomHistory.Add(PhotonNetwork.CurrentRoom.Name + $" [<color=green>{Tools.Utilities.GetCurrentGamemode()}</color>]");
        }

        public static void OnLeave()
        {
            Debug.Log("cleaning up");
            Mods.Visuals.ClearLinePool(true);
            ExternalObject.DestroyAll();
        }

        public static void OnOtherPlayerJoin(Player other)
        {
            ExternalObject.SyncToPlayer(other);
        }

        public static void OnOtherPlayerLeave(Player other) 
        { 

        }
    }
}