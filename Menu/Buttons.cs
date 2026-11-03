using GorillaLocomotion;
using GorillaNetworking;
using externallol.Classes;
using externallol.Classes.Network;
using externallol.Mods;
using externallol.Notifications;
using externallol.Patches;
using externallol.Patches.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static externallol.Menu.Main;
namespace externallol.Menu
{
    // hi hi this is buttons class its sigma anyway
    // commented out mods are part of the cemetery for mods that are not in for some reason or just dont work
    public static class Buttons
    {
        public static ButtonInfo[] MainMods =
        [
            new() { buttonText = "Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the settings." },
            new() { buttonText = "Join Discord", method = () => Process.Start("https://discord.gg/bmk4VzvcZc"), mode = ButtonMode.Action, toolTip = "Joins the menu's Discord server." },
            new() { buttonText = "Room Mods", method = () => SetCategory("Room Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the room mods category." },
            new() { buttonText = "Movement Mods", method = () => SetCategory("Movement Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the movement mods category." },
            new() { buttonText = "Enabled Mods", method = () => { UpdateEnabledMods(); SetCategory("Enabled Mods"); }, mode = ButtonMode.Action, isCategory = true, toolTip = "Shows currently enabled mods." },
            new() { buttonText = "Safety Mods", method = () => SetCategory("Safety Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the safety mods category." },
            new() { buttonText = "Visual Mods", method = () => SetCategory("Visual Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the visual mods category." },
            new() { buttonText = "Player Mods", method = () => SetCategory("Player Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the player mods category." },
            new() { buttonText = "Fun Mods", method = () => SetCategory("Fun Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the fun mods category." },
            new() { buttonText = "Overpowered Mods", method = () => SetCategory("Overpowered Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the overpowered mods category." },
            new() { buttonText = "Advantage Mods", method = () => SetCategory("Advantage Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the advantage mods category." },
            new() { buttonText = "Important Mods", method = () => SetCategory("Important Mods"), mode = ButtonMode.Action, isCategory = true, toolTip = "Opens the important mods category." },
        ];

        public static ButtonInfo[] Settings =
            [
                new() { buttonText = "Back", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main menu page." },

                new() { buttonText = "Menu Settings", method = () => SetCategory("Menu Settings"), mode = ButtonMode.Action, toolTip = "Opens the menu settings." },
                new() { buttonText = "Movement Settings", method = () => SetCategory("Movement Settings"), mode = ButtonMode.Action, toolTip = "Opens the movement settings." },
                new() { buttonText = "Advantage Settings", method = () => SetCategory("Advantage Settings"), mode = ButtonMode.Action, toolTip = "Opens the advantage settings." },
            ];

        private static readonly ButtonInfo[] MenuSettings =
        [
            new() { buttonText = "Back", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main settings page." },

                new() { buttonText = "Clear Notifications", method = Notifications.NotificationManager.ClearAllNotifications, mode = ButtonMode.Action, toolTip = "Clears all notifications from the menu."},
                new() { buttonText = "Right Handed Menu", enableMethod = () => Menu.Settings.rightHanded = true, disableMethod = () => Menu.Settings.rightHanded = false, enabled = Menu.Settings.rightHanded, mode = ButtonMode.Toggle, toolTip = "Moves the menu to your right hand." },
                new() { buttonText = "COC Room History", enableMethod = RoomHistory.Enable, disableMethod = RoomHistory.Disable, toolTip = "Displays recent rooms on the COC board."},
                new() { buttonText = "Disable External", enableMethod =() => External.ExternalEnabled = false, disableMethod =() => External.ExternalEnabled = true, toolTip = "Disables External's networking"},
                new() { buttonText = "Disable Notifications", enableMethod = () => Menu.Settings.disableNotifications = true, disableMethod = () => Menu.Settings.disableNotifications = false, enabled = Menu.Settings.disableNotifications, mode = ButtonMode.Toggle, toolTip = "Disables notifications." },
                new() { buttonText = "Disable FPS Counter", enableMethod = () => Menu.Settings.fpsCounter = false, disableMethod = () => Menu.Settings.fpsCounter = true, enabled = !Menu.Settings.fpsCounter, mode = ButtonMode.Toggle, toolTip = "Disables the FPS counter." },
                new() { buttonText = "Disable Disconnect Button", enableMethod = () => Menu.Settings.disconnectButton = false, disableMethod = () => Menu.Settings.disconnectButton = true, enabled = !Menu.Settings.disconnectButton, mode = ButtonMode.Toggle, toolTip = "Disables the disconnect button." },
                new() { buttonText = "Disable Home Button", enableMethod = () => Menu.Settings.homeButton = false, disableMethod = () => Menu.Settings.homeButton = true, enabled = !Menu.Settings.homeButton, mode = ButtonMode.Toggle, toolTip = "Disables the home button." },
                new() { buttonText = "Incremental Buttons", enableMethod = () => Menu.Settings.incrementalButtons = true, disableMethod = () => Menu.Settings.incrementalButtons = false, enabled = Menu.Settings.incrementalButtons, mode = ButtonMode.Toggle, toolTip = "Shows separate minus and plus controls for incremental settings." },
                new() { buttonText = "Rounded Menu", enableMethod = () => Menu.Settings.roundedButtons = true, disableMethod = () => Menu.Settings.roundedButtons = false, enabled = Menu.Settings.roundedButtons, mode = ButtonMode.Toggle, toolTip = "Rounds the menu, buttons, search keyboard, and other menu elements." },
                new() { buttonText = "Drop Menu", enableMethod = () =>Menu.Settings. dropMenu = true, disableMethod = () => Menu.Settings.dropMenu = false, enabled = Menu.Settings.dropMenu, mode = ButtonMode.Toggle, toolTip = "Opens the menu while held and drops it when released." },
                new() { buttonText = "Animated Menu", enableMethod = () => Menu.Settings.animateMenu = true, disableMethod = () => Menu.Settings.animateMenu = false, enabled = Menu.Settings.animateMenu, mode = ButtonMode.Toggle, toolTip = "Adds a simple grow and shrink animation to the menu." },
                new() { buttonText = "Gradient Animation", incrementMethod = Menu.Settings.ChangeGradientAnimation, displayText = () => $"Gradient Animation <color=grey>[</color><color=green>{Menu.Settings.GradientAnimationName}</color><color=grey>]</color>", mode = ButtonMode.Incremental, toolTip = "Changes how the animated gradients move." },
                new() { buttonText = "Rainbow Colours", enableMethod = () => Menu.Settings.SetRainbowColors(true), disableMethod = () => Menu.Settings.SetRainbowColors(false), enabled = Menu.Settings.rainbowColors, mode = ButtonMode.Toggle, toolTip = "Uses animated rainbow colours instead of the selected colour scheme." },
                new() { buttonText = "Theme Changer", incrementMethod = Menu.Settings.ChangeTheme, displayText = () => $"Theme Changer <color=grey>[</color><color=green>{Menu.Settings.ThemeName}</color><color=grey>]</color>", mode = ButtonMode.Incremental, toolTip = "Changes the color scheme used by the menu." },
                new() { buttonText = "Outlines", enableMethod = () => Menu.Settings.outlines = true, disableMethod = () => Menu.Settings.outlines = false, enabled = Menu.Settings.outlines, mode = ButtonMode.Toggle, toolTip = "Adds outlines around the menu and its buttons." },
                new() { buttonText = "Button Gradients", enableMethod = () => Menu.Settings.buttonGradients = true, disableMethod = () => Menu.Settings.buttonGradients = false, enabled = Menu.Settings.buttonGradients, mode = ButtonMode.Toggle, toolTip = "Toggles spatial gradients on buttons." },
                new() { buttonText = "Vertical Gradients", enableMethod = () => Menu.Settings.verticalButtonGradients = true, disableMethod = () => Menu.Settings.verticalButtonGradients = false, enabled = Menu.Settings.verticalButtonGradients, mode = ButtonMode.Toggle, toolTip = "Switches button gradients between horizontal and vertical." },
                new() { buttonText = "Top Page Buttons", enableMethod = () => Menu.Settings.pageButtonsAtTop = true, disableMethod = () => Menu.Settings.pageButtonsAtTop = false, enabled = Menu.Settings.pageButtonsAtTop, mode = ButtonMode.Toggle, toolTip = "Moves the page buttons into the first two normal button slots." },
            ];

        public static ButtonInfo[] MovementSettings =
        [
            new() { buttonText = "Back", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main settings page." },

                new() { buttonText = "Change Fly Speed", overlapText = $"Change Fly Speed <color=grey>[</color><color=green>{Mods.Settings.Movement.flySpeedNames[Mods.Settings.Movement.flySpeedIndex]}</color><color=grey>]</color>", incrementMethod = Mods.Settings.Movement.ChangeFlySpeed, mode = ButtonMode.Incremental, toolTip = "Changes the fly speed." },
                new() { buttonText = "Change Arm Length", overlapText = $"Change Arm Length <color=grey>[</color><color=green>{Mods.Settings.Movement.armLengthNames[Mods.Settings.Movement.armLengthIndex]}</color><color=grey>]</color>", incrementMethod = Mods.Settings.Movement.ChangeArmLength, mode = ButtonMode.Incremental, toolTip = "Changes the length of your arms." },
                new() { buttonText = "Change Platform Shape", overlapText = $"Change Platform Shape <color=grey>[</color><color=green>{Mods.Settings.Movement.platformShapeNames[Mods.Settings.Movement.platformShapeIndex]}</color><color=grey>]</color>", incrementMethod = Mods.Settings.Movement.ChangePlatformShape, mode = ButtonMode.Incremental, toolTip = "Changes the shape of your platforms." },
                new() { buttonText = "Change Speed Boost Amount", overlapText = $"Change Speed Boost Amount <color=grey>[</color><color=green>{Mods.Settings.Movement.speedNames[Mods.Settings.Movement.speedAmountIndex]}</color><color=grey>]</color>", incrementMethod = Mods.Settings.Movement.ChangeSpeedBoostAmount, mode = ButtonMode.Incremental, toolTip = "Changes the amount of your speed boost." },
                new() { buttonText = "Grip Speed Boost", enableMethod = () => gripSpeedBoost = true, disableMethod = () => gripSpeedBoost = false, mode = ButtonMode.Toggle, toolTip = "Binds the speed boost mod to your grip." },
                new() { buttonText = "Trigger Speed Boost", enableMethod = () => triggerSpeedBoost = true, disableMethod = () => triggerSpeedBoost = false, mode = ButtonMode.Toggle, toolTip = "Binds the speed boost mod to your trigger." },
                new() { buttonText = "Trigger Platforms", enableMethod = () => triggerPlatforms = true, disableMethod = () => triggerPlatforms = false, mode = ButtonMode.Toggle, toolTip = "Spawns platforms on your hands when you press trigger." },
                new() { buttonText = "Non-Sticky Platforms", enableMethod = () => nonStickyPlatforms = true, disableMethod = () => nonStickyPlatforms = false, mode = ButtonMode.Toggle, toolTip = "Prevents your hands from sticking to platforms." },
                new() { buttonText = "Constant Noclip", enableMethod = () => constantNoclip = true, disableMethod = () => constantNoclip = false, mode = ButtonMode.Toggle, toolTip = "Keeps the noclip mod always on." },
                new() { buttonText = "Grip Noclip", enableMethod = () => gripNoclip = true, disableMethod = () => gripNoclip = false, mode = ButtonMode.Toggle, toolTip = "Binds the noclip mod to your grip." },
            ];
        public static ButtonInfo[] AdvantageSettings =
        [
            new() { buttonText = "Back", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main settings page." },
            new() { buttonText = "Obnoxious Tag", method = () => Main.Nothing(), mode = ButtonMode.Toggle, toolTip = "Makes Tag Mods obnoxious." }
        ];

        public static ButtonInfo[] RoomMods =
        [
            new() { buttonText = "Exit Room Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main menu page." },

                new() { buttonText = "Disconnect", method = () => NetworkSystem.Instance.ReturnToSinglePlayer(), mode = ButtonMode.Action, toolTip = "Disconnects you from the current room." },
                new() { buttonText = "Join Last Room", method = Room.JoinLastRoom, mode = ButtonMode.Action, toolTip = "Joins the last room you were in." },
                new() { buttonText = "Join Random Public Room", method = Room.JoinRandomRoom, mode = ButtonMode.Action, toolTip = "Joins a random available public room." },
                new() { buttonText = "Join Random Private Room", method = Room.JoinRandomPriv, mode = ButtonMode.Action, toolTip = "Joins a random available private room." },
                new() { buttonText = "Reconnect", method = Room.Reconnect, mode = ButtonMode.Action, toolTip = "Reconnects you to the Photon server." },
                new() { buttonText = "Join Menu Room", method = Room.JoinMenuRoom, mode = ButtonMode.Action, toolTip = "Joins the BRANDED menu room." },
                new() { buttonText = "Check Master", method = Room.AmIMaster, mode = ButtonMode.Action, toolTip = "Checks if you are the master client of the room." },
                new() { buttonText = "Queue Rooms", method =() => SetCategory("Queue Room Mods"), mode = ButtonMode.Action, toolTip = "Opens the Queue Rooms tab."}
        ];

        public static ButtonInfo[] QueueRoomMods =
        [
            new() { buttonText = "Exit Queue Room Mods", method = () => SetCategory("Room Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the room mods page of the menu." },

                new() { buttonText = "Queue Room <color=grey>[</color><color=green>MODS</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("mods"), mode = ButtonMode.Action, toolTip = "Joins the MODS queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>MOD</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("mod"), mode = ButtonMode.Action, toolTip = "Joins the MOD queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>MODDER</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("modder"), mode = ButtonMode.Action, toolTip = "Joins the MODDER queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>BOT</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("bot"), mode = ButtonMode.Action, toolTip = "Joins the BOT queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>DAISY09</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("daisy09"), mode = ButtonMode.Action, toolTip = "Joins the DAISY09 queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>DAISY</color><color=grey>]</color>", method =() => Room.QueueJoinRoom("daisy"), mode = ButtonMode.Action, toolTip = "Joins the DAISY queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>PBBV</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("pbbv"), mode = ButtonMode.Action, toolTip = "Joins the PBBV queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>RUN</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("run"), mode = ButtonMode.Action, toolTip = "Joins the RUN queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>J3VU</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("j3vu"), mode = ButtonMode.Action, toolTip = "Joins the J3VU queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>GHOST</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("ghost"), mode = ButtonMode.Action, toolTip = "Joins the GHOST queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>TIPTOE</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("tiptoe"), mode = ButtonMode.Action, toolTip = "Joins the TIPTOE queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>SREN17</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("sren17"), mode = ButtonMode.Action, toolTip = "Joins the SREN17 queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>SREN18</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("sren18"), mode = ButtonMode.Action, toolTip = "Joins the SREN18 queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>SREN16</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("sren16"), mode = ButtonMode.Action, toolTip = "Joins the SREN16 queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>HELP</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("help"), mode = ButtonMode.Action, toolTip = "Joins the HELP queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>HIDE</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("hide"), mode = ButtonMode.Action, toolTip = "Joins the HIDE queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>H3LP</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("h3lp"), mode = ButtonMode.Action, toolTip = "Joins the H3LP queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>BANSHEE</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("banshee"), mode = ButtonMode.Action, toolTip = "Joins the BANSHEE queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>PAUL</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("paul"), mode = ButtonMode.Action, toolTip = "Joins the PAUL queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>ECHO</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("echo"), mode = ButtonMode.Action, toolTip = "Joins the ECHO queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>WARNING</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("warning"), mode = ButtonMode.Action, toolTip = "Joins the WARNING queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>CHEATS</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("cheats"), mode = ButtonMode.Action, toolTip = "Joins the CHEATS queue room." },
                new() { buttonText = "Queue Room <color=grey>[</color><color=green>LUCIO</color><color=grey>]</color>", method = () => Room.QueueJoinRoom("lucio"), mode = ButtonMode.Action, toolTip = "Joins the LUCIO queue room." },
            ];

        public static ButtonInfo[] MovementMods =
        [
            new() { buttonText = "Exit Movement Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main menu page." },

                new() { buttonText = "Platforms <color=grey>[</color><color=green>G</color><color=grey>]</color>", method = Movement.Platforms, mode = ButtonMode.Toggle, toolTip = "Spawns platforms on your hands when you press grip." },
                new() { buttonText = "Frozone <color=grey>[</color><color=green>G</color><color=grey>]</color>", method = Movement.Frozone, toolTip = "Spawns slippery blocks under your hands using <color=green>grip</color>."},
                new() { buttonText = "Fly <color=grey>[</color><color=green>A</color><color=grey>]</color>", method = Movement.Fly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding A." },
                new() { buttonText = "Hand Fly <color=grey>[</color><color=green>A</color><color=grey>]</color>", method = Movement.HandFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward in the direction your hand is pointing while holding A." },
                new() { buttonText = "Noclip Fly <color=grey>[</color><color=green>A</color><color=grey>]</color>", method = Movement.NoclipFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding A and lets you phase through walls." },
                new() { buttonText = "Trigger Fly <color=grey>[</color><color=green>T</color><color=grey>]</color>", method = Movement.TriggerFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding your right trigger." },
                new() { buttonText = "WASD Fly", enableMethod = Movement.EnableWASDFly, method = Movement.WASDFly, disableMethod = () => GTPlayer.Instance.GetControllerTransform(false).parent.rotation = Quaternion.Euler(0, 0, 0), mode = ButtonMode.Toggle, toolTip = "Moves you around with WASD." },
                //new() { buttonText = "Bark Fly <color=grey>[</color><color=green>BK</color><color=grey>]</color>", enableMethod = Movement.BarkFly, mode = ButtonMode.Toggle, toolTip = "Moves you around with your joysticks. Credit: KyleTheScientist" },
                new() { buttonText = "Teleport Gun", method = Movement.TeleportGun, mode = ButtonMode.Toggle, toolTip = "Teleports you to the pointer position when you press the trigger." },
                new() { buttonText = "Speed Boost", method = Movement.SpeedBoost, mode = ButtonMode.Toggle, toolTip = "Increases your movement speed." },
                new() { buttonText = "Noclip <color=grey>[</color><color=green>T</color><color=grey>]</color>", method = Movement.Noclip, mode = ButtonMode.Toggle, toolTip = "Allows you to move through walls." },
                new() { buttonText = "Steam Long Arms", enableMethod = Movement.EnableSteamLongArms, disableMethod = Movement.DisableSteamLongArms, method = Movement.EnableSteamLongArms, mode = ButtonMode.Toggle, toolTip = "Simulates SteamVR's world scale to make your arms longer." },
                //new() { buttonText = "Stick Long Arms", enableMethod = Movement.StickLongArms, mode = ButtonMode.Toggle, toolTip = "Simulates arm extenders." },
                new() { buttonText = "Sticky Hands", enableMethod = Movement.StickyHands, disableMethod = Movement.DisableStickyHands, method = Movement.StickyHands, mode = ButtonMode.Toggle, toolTip = "Makes your hands sticky." },
                new() { buttonText = "Slide Control", enableMethod = Movement.EnableSlideControl, disableMethod = Movement.DisableSlideControl, mode = ButtonMode.Toggle, toolTip = "Allows you to control your sliding." },
                new() { buttonText = "Moon Walk", method = Movement.LowGravity, mode = ButtonMode.Toggle, toolTip = "Reduces the gravity affecting you." },
                new() { buttonText = "No Gravity", method = Movement.ZeroGravity, mode = ButtonMode.Toggle, toolTip = "Removes gravity from you." },
                new() { buttonText = "Jupiter Walk", method = Movement.HighGravity, mode = ButtonMode.Toggle, toolTip = "Increases the gravity affecting you." },
                new() { buttonText = "Up And Down", method = Movement.UpAndDown, mode = ButtonMode.Toggle, toolTip = "Moves you up and down." },
                new() { buttonText = "Left And Right", method = Movement.LeftAndRight, mode = ButtonMode.Toggle, toolTip = "Moves you left and right." },
                new() { buttonText = "Forwards And Backwards", method = Movement.ForwardsAndBackwards, mode = ButtonMode.Toggle, toolTip = "Moves you forwards and backwards." },
                new() { buttonText = "Force Tag Freeze", method = Movement.ForceTagFreeze, mode = ButtonMode.Toggle, toolTip = "Forces tag freeze." },
                new() { buttonText = "No Tag Freeze", method = Movement.NoTagFreeze, mode = ButtonMode.Toggle, toolTip = "Removes tag freeze." },
                new() { buttonText = "Wall Walk <color=grey>[</color><color=green>G</color><color=grey>]</color>", method = Movement.WallWalk, mode = ButtonMode.Toggle, toolTip = "Allows you to walk on walls." },
            ];

        public static ButtonInfo[] SafetyMods =
        [
            new() { buttonText = "Exit Safety Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },

                new() { buttonText = "Anti-Report <color=grey>[</color><color=green>Disconnect</color><color=grey>]</color>", method = Safety.AntiReportDisconnect, mode = ButtonMode.Toggle, toolTip = "Disconnects you when you are reported." },
                new() { buttonText = "Anti-Report <color=grey>[</color><color=green>Quest</color><color=grey>]</color>", enableMethod = Safety.EnableAntiOculusReport, disableMethod = Safety.DisableAntiOculusReport, toolTip = "Disconnects when you are reported through the Meta Quest report menu."},
                new() { buttonText = "Anti-Report <color=grey>[</color><color=green>Notify</color><color=grey>]</color>", enableMethod = Safety.AntiReportNotify, toolTip = "Notifies you when you are reported."},

                new() { buttonText = "Anti-Report <color=grey>[</color><color=green>VIM Kick</color><color=grey>]</color>", method =() => Safety.VIMAntiReport(false), mode = ButtonMode.Toggle, toolTip = "Kicks the player who attempts to report you."},
                new() { buttonText = "Anti-Report <color=grey>[</color><color=green>VIM Block</color><color=grey>]</color>", method =() => Safety.VIMAntiReport(true), mode = ButtonMode.Toggle, toolTip = "Blocks the player who attempts to report you."},

                new() { buttonText = "Show Anti Cheat Reports <color=grey>[</color><color=green>Self</color><color=grey>]</color>", enableMethod =() => AntiCheatPatches.SendReportPatch.AntiCheatSelf = true, disableMethod =() => AntiCheatPatches.SendReportPatch.AntiCheatSelf = false, toolTip = "Gives you a notification every time you have been reported by the anti cheat."},
                new() { buttonText = "Show Anti Cheat Reports <color=grey>[</color><color=green>All</color><color=grey>]</color>", enableMethod =() => AntiCheatPatches.SendReportPatch.AntiCheatAll = true, disableMethod =() => AntiCheatPatches.SendReportPatch.AntiCheatAll = false, toolTip = "Gives you a notification every time anyone has been reported by the anti cheat."},

                new() { buttonText = "Anti-Moderator", method = Safety.AntiModerator, mode = ButtonMode.Toggle, toolTip = "Disconnects when there is a moderator in the lobby." },
                new() { buttonText = "No Finger Movement", method = Safety.NoFingerMovement, mode = ButtonMode.Toggle, toolTip = "Disables finger movement." },
                new() { buttonText = "Flush RPCs", method = Safety.FlushRPCs, mode = ButtonMode.Action, toolTip = "Flushes RPCs." },
            ];

        public static ButtonInfo[] VisualMods =
        [
            new() { buttonText = "Exit Visual Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },

                new() { buttonText = "Bone ESP", method = Visuals.BoneESP, disableMethod = Visuals.DisableBoneESP, mode = ButtonMode.Toggle, toolTip = "Displays other players’ bones." },
                new() { buttonText = "Box ESP", method = Visuals.CasualBoxESP, disableMethod = Visuals.DisableBoxESP, mode = ButtonMode.Toggle, toolTip = "Displays boxes over other players." },
                new() { buttonText = "Tracers", method = Visuals.Tracers, disableMethod =() => {Visuals.isLineRenderQueued = false; Visuals.ClearLinePool(); }, mode = ButtonMode.Toggle, toolTip = "Draws lines from your hand to other players." },
                new() { buttonText = "Morning Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(1), toolTip = "Sets your time of day to morning."},
                new() { buttonText = "Day Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(3), toolTip = "Sets your time of day to daytime."},
                new() { buttonText = "Evening Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(7), toolTip = "Sets your time of day to evening."},
                new() { buttonText = "Night Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(0), toolTip = "Sets your time of day to night."},
                new() { buttonText = "Remove Leaves", method = Visuals.EnableRemoveLeaves, disableMethod = Visuals.DisableRemoveLeaves, toolTip = "Disables all leaves."},
                new() { buttonText = "Enable Unbaked Cherry Blossoms", enableMethod = Visuals.CherryBlossoms, disableMethod = Visuals.DisableCherryBlossoms, toolTip = "Enables cherry blossoms."},
            ];

        public static ButtonInfo[] PlayerMods =
        [
            new() { buttonText = "Exit Player Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },

                new() { buttonText = "Fix Head", method = Player.FixHead, mode = ButtonMode.Action, toolTip = "Fixes issues with your head." },
                new() { buttonText = "Grab Rig <color=grey>[</color><color=green>G</color><color=grey>]</color>", method = Player.GrabRig, mode = ButtonMode.Toggle, toolTip = "Lets you grab your rig." },
                new() { buttonText = "Ghostview", method = Player.GhostView, mode = ButtonMode.Toggle, toolTip = "Spawns two spheres on your hands when using Ghost or Invisible." },
                new() { buttonText = "Ghost <color=grey>[</color><color=green>A</color><color=grey>]</color>", method = Player.Ghost, mode = ButtonMode.Toggle, toolTip = "Makes you a ghost." },
                new() { buttonText = "Invisible <color=grey>[</color><color=green>B</color><color=grey>]</color>", method = Player.Invisible, mode = ButtonMode.Toggle, toolTip = "Makes you invisible." },
                new() { buttonText = "Spin Head X", method = () => Player.SpinHead("x"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the X axis." },
                new() { buttonText = "Spin Head Y", method = () => Player.SpinHead("Y"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the Y axis." },
                new() { buttonText = "Spin Head Z", method = () => Player.SpinHead("Z"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the Z axis." },
                new() { buttonText = "Backwards Head", method = Player.BackwardsHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head backwards." },
                new() { buttonText = "Sideways Head", method = Player.SidewaysHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head sideways." },
                new() { buttonText = "Upside Down Head", method = Player.UpsideDownHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head upside down." },
                new() { buttonText = "Spaz Head X", method = () => Player.SpazHead("x"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the X axis." },
                new() { buttonText = "Spaz Head Y", method = () => Player.SpazHead("y"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the Y axis." },
                new() { buttonText = "Spaz Head Z", method = () => Player.SpazHead("z"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the Z axis." },
                new() { buttonText = "Spaz Head", method = Player.SpazHeadXYZ, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Spazzes your head on all axes." },
                new() { buttonText = "Broken Neck", method = Player.BrokenNeck, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Breaks your neck." },
                new() { buttonText = "Head Bang", method = Player.HeadBang, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Makes your head bang." },
            ];


        public static ButtonInfo[] AdvantageMods =
        [
                new() { buttonText = "Exit Advantage Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },

                new() { buttonText = "Tag Gun", method = Advantages.TagGun, mode = ButtonMode.Toggle, toolTip = "Tags whoever your hand desires."},
                new() { buttonText = "Tag All <color=grey>[</color><color=green>MD</color><color=grey>]</color>", method = Advantages.TagAll, mode = ButtonMode.Toggle, toolTip = "Tags everyone in the room." },
                new() { buttonText = "Untag All <color=grey>[</color><color=green>M</color><color=grey>]</color>", method = Advantages.UntagAll, mode = ButtonMode.Action, toolTip = "Removes the tagged state from everyone." },
                new() { buttonText = "Untag Gun <color=grey>[</color><color=green>M</color><color=grey>]</color>", method = Advantages.UntagGun, mode = ButtonMode.Toggle, toolTip = "Removes the tagged state from the player you target with the gun." },
                new() { buttonText = "Disable Tags <color=grey>[</color><color=green>M</color><color=grey>]</color>", method = Advantages.DisableTags, mode = ButtonMode.Toggle, disableMethod = Notifications.NotificationManager.ClearAllNotifications, toolTip = "Continuously removes the tagged state from everyone." },
                //new() { buttonText = "Flick Tag Gun <color=grey>[</color><color=green>BK</color><color=grey>]</color>", method = Advantages.FlickTagGun, mode = ButtonMode.Toggle, toolTip = "Simulates a flick tag." },
            ];

        public static ButtonInfo[] ImportantMods =
        [
            new() { buttonText = "Exit Important Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },
                new() { buttonText = "Crash", method = Application.Quit, mode = ButtonMode.Action, toolTip = "Closes the game." },
            ];

        public static ButtonInfo[] VIMMods =
        [
            new() { buttonText = "Exit VIM Mods", method = () => SetCategory("Overpowered Mods"), mode = ButtonMode.Action, toolTip = "Returns to the overpowered mods page." },

                new() { buttonText = "VIM Clear Blocked", method =() => RoomControls.blockedPlayers.Clear(), mode = ButtonMode.Toggle, toolTip = "Clears blocked players. [VIM]" },
                new() { buttonText = "VIM Kick Gun", method =() => OP.VIMActionGun(0), mode = ButtonMode.Toggle, toolTip = "Kicks the player you target. [VIM]" },
                new() { buttonText = "VIM Block Gun", method =() => OP.VIMActionGun(1), mode = ButtonMode.Toggle, toolTip = "Blocks the player you target. [VIM]" },
                new() { buttonText = "VIM Mute Gun", method =() => OP.VIMActionGun(2), mode = ButtonMode.Toggle, toolTip = "Mutes the player you target. [VIM]" },
                new() { buttonText = "VIM Kick All", method =() => OP.VIMActionAll(0), mode = ButtonMode.Toggle, toolTip = "Kicks everyone in the room. [VIM]" },
                new() { buttonText = "VIM Block All", method =() => OP.VIMActionAll(1), mode = ButtonMode.Toggle, toolTip = "Blocks everyone in the room. [VIM]" },
                new() { buttonText = "VIM Mute All", method =() => OP.VIMActionAll(2), mode = ButtonMode.Toggle, toolTip = "Mutes everyone in the room. [VIM]" },
                new() { buttonText = "VIM Kick On Touch", method =() => OP.VIMActionOnTouch(0), mode = ButtonMode.Toggle, toolTip = "Kicks whoever your hand collides with. [VIM]" },
                new() { buttonText = "VIM Block On Touch", method =() => OP.VIMActionOnTouch(1), mode = ButtonMode.Toggle, toolTip = "Blocks whoever your hand collides with. [VIM]" },
                new() { buttonText = "VIM Mute On Touch", method =() => OP.VIMActionOnTouch(2), mode = ButtonMode.Toggle, toolTip = "Mutes whoever your hand collides with. [VIM]" },
                new() { buttonText = "VIM Kick All Tagged", method = () => OP.VIMActionTagged(0), mode = ButtonMode.Toggle, toolTip = "Kicks every tagged player. [VIM]" },
                new() { buttonText = "VIM Block All Tagged", method = () => OP.VIMActionTagged(1), mode = ButtonMode.Toggle, toolTip = "Blocks every tagged player. [VIM]" },
                new() { buttonText = "VIM Mute All Tagged", method = () => OP.VIMActionTagged(2), mode = ButtonMode.Toggle, toolTip = "Mutes every tagged player. [VIM]" },
                new() { buttonText = "VIM Kick All Untagged", method = () => OP.VIMActionUntagged(0), mode = ButtonMode.Toggle, toolTip = "Kicks every tagged player. [VIM]" },
                new() { buttonText = "VIM Block All Untagged", method = () => OP.VIMActionUntagged(1), mode = ButtonMode.Toggle, toolTip = "Blocks every tagged player. [VIM]" },
                new() { buttonText = "VIM Mute All Untagged", method = () => OP.VIMActionUntagged(2), mode = ButtonMode.Toggle, toolTip = "Mutes every tagged player. [VIM]" },
                new() { buttonText = "VIM Kick Random", method = () => OP.VIMActionRandom(0), mode = ButtonMode.Toggle, toolTip = "Kicks a random player. [VIM]" },
                new() { buttonText = "VIM Block Random", method = () => OP.VIMActionRandom(1), mode = ButtonMode.Toggle, toolTip = "Blocks a random player. [VIM]" },
                new() { buttonText = "VIM Mute Random", method = () => OP.VIMActionRandom(2), mode = ButtonMode.Toggle, toolTip = "Mutes a random player. [VIM]" },
                new() { buttonText = "VIM Kick Closest", method = () => OP.VIMActionClosest(0), mode = ButtonMode.Toggle, toolTip = "Kicks the closest player. [VIM]" },
                new() { buttonText = "VIM Block Closest", method = () => OP.VIMActionClosest(1), mode = ButtonMode.Toggle, toolTip = "Blocks the closest player. [VIM]" },
                new() { buttonText = "VIM Mute Closest", method = () => OP.VIMActionClosest(2), mode = ButtonMode.Toggle, toolTip = "Mutes the closest player. [VIM]" },
            ];

        public static ButtonInfo[] OpMods =
        [
            new() { buttonText = "Exit Overpowered Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },

                new() { buttonText = "Get Free VIM", enableMethod =() => SubscriptionPatches.enabled = true, disableMethod =() => SubscriptionPatches.enabled = false, toolTip = "Unlocks VIM." },
                new() { buttonText = "VIM Mods", method =() => SetCategory("VIM"), mode = ButtonMode.Action, toolTip = "Opens the VIM mods tab."},

                new() { buttonText = "Leak Live Event <color=grey>[</color><color=green>CS</color><color=grey>]</color>", method = OP.LeakLive, mode = ButtonMode.Action, toolTip = "Leaks the latest live event."},
                new() { buttonText = "Stump Kick Gun", method = OP.StumpKickGun, mode = ButtonMode.Toggle, toolTip = "Allows you to kick players within Stump with a gun."},
                new() { buttonText = "Stump Kick All", method = OP.StumpKickAll, mode = ButtonMode.Action, toolTip = "Kicks everyone in Stump."},
                //new() { buttonText = "Slow Gun <color=grey>[</color><color=green>M</color><color=grey>]</color> <color=grey>[</color><color=green>BK</color><color=grey>]</color>", method = OP.SlowGun, mode = ButtonMode.Toggle, toolTip = "Slows the player you target." },
                //new() { buttonText = "Vibrate Gun <color=grey>[</color><color=green>M</color><color=grey>]</color> <color=grey>[</color><color=green>BK</color><color=grey>]</color>", method = OP.VibrateGun, mode = ButtonMode.Toggle, toolTip = "Vibrates the player you target." },
                new() { buttonText = "Rock To Infection", method = OP.TagToInfection, mode = ButtonMode.Action, toolTip = "Changes the game mode." },
                new() { buttonText = "Infection To Rock", method = OP.InfectionToTag, mode = ButtonMode.Action, toolTip = "Changes the game mode." },
                new() { buttonText = "Spaz Ropes", method = OP.SpazAllRopes, mode = ButtonMode.Toggle, toolTip = "Rapidly moves all ropes." },
                new() { buttonText = "Spaz Grabbed Ropes", method = OP.SpazGrabbedRopes, mode = ButtonMode.Toggle, toolTip = "Rapidly moves grabbed ropes." },
                new() { buttonText = "Spaz Rope Gun", method = OP.SpazRopeGun, mode = ButtonMode.Toggle, toolTip = "Rapidly moves the rope you target." },
                new() { buttonText = "Fling Rope Gun", method = OP.FlingRopeGun, mode = ButtonMode.Toggle, toolTip = "Flings the rope you target." },
                new() { buttonText = "Fling All Ropes Gun", method = OP.FlingAllRopesGun, mode = ButtonMode.Toggle, toolTip = "Flings all ropes." },
                new() { buttonText = "Spawn Blue Lucy", method = OP.SpawnBlueLucy, mode = ButtonMode.Toggle, toolTip = "Spawns a Blue Lucy." },
                new() { buttonText = "Spawn Red Lucy", method = OP.SpawnRedLucy, mode = ButtonMode.Toggle, toolTip = "Spawns a Red Lucy." },
                //new() { buttonText = "Spawn Lurker", method = OP.SpawnLurker, mode = ButtonMode.Toggle, toolTip = "Spawns the Lurker." },
                new() { buttonText = "Lucy Attack Gun", method = OP.LucyAttackGun, mode = ButtonMode.Toggle, toolTip = "Makes Lucy attack whoever you target." },
                new() { buttonText = "Lucy Chase Gun", method = OP.LucyChaseGun, mode = ButtonMode.Toggle, toolTip = "Makes Lucy chase whoever you target." },
                //new() { buttonText = "Lurker Attack Gun", method = OP.LurkerAttackGun, mode = ButtonMode.Toggle, toolTip = "Makes the Lurker attack whoever you target." },
                //new() { buttonText = "Lurker Move Gun", method = OP.MoveLurkerGun, mode = ButtonMode.Toggle, toolTip = "Makes the Lurker move wherever you target." },
                new() { buttonText = "Kill Lucy <color=grey>[</color><color=green>SS!</color><color=grey>]</color>", method = OP.DespawnLucy, mode = ButtonMode.Action, toolTip = "Kills Lucy, suprisingly server sided." },
                //new() { buttonText = "Kill Lurker", method = OP.DespawnLurker, mode = ButtonMode.Action, toolTip = "Kills the Lurker." },
                //new() { buttonText = "Delay Ban Gun <color=grey>[</color><color=green>D?</color><color=grey>]</color>", method = OP.DelayBanGun, mode = ButtonMode.Toggle, toolTip = "Bans the targeted player for cheating."},
                //new() { buttonText = "Crash Gun", method = OP.CrashGun, mode = ButtonMode.Toggle, toolTip = "Poo" },
            ];

        public static ButtonInfo[] FunMods =
        [
            new() { buttonText = "Exit Fun Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },
                new() { buttonText = "Water Bending <color=grey>[</color><color=green>RG</color><color=grey>]</color>", method = Fun.WaterR, mode = ButtonMode.Toggle, toolTip = "Spawns water splashes at your right hand." },
                new() { buttonText = "Water Bending <color=grey>[</color><color=green>LG</color><color=grey>]</color>", method = Fun.WaterL, mode = ButtonMode.Toggle, toolTip = "Spawns water splashes at your left hand." },
                new() { buttonText = "Water Splash Gun", method = Fun.WaterSplashGun, mode = ButtonMode.Toggle, toolTip = "Creates water splashes wherever your gun points." },
                new() { buttonText = "Water Splash Aura", method = Fun.WaterSplashAura, mode = ButtonMode.Toggle, toolTip = "Creates water splashes randomly around you." },
                new() { buttonText = "Water Splash <color=grey>[</color><color=green>G</color><color=grey>]</color>", method = Fun.WaterSplashHands, mode = ButtonMode.Toggle, toolTip = "Makes your hands create water splashes." },
                new() { buttonText = "Water Splash Walk", method = Fun.WaterSplashWalk, mode = ButtonMode.Toggle, toolTip = "Creates water splashes as you walk." },
                new() { buttonText = "Water Splash On Touch", method = Fun.WaterSplashOnTouch, mode = ButtonMode.Toggle, toolTip = "Creates water splashes on players you touch." },
                new() { buttonText = "Give Water Splash Hands Gun", method = Fun.GiveWaterSplashHandsGun, mode = ButtonMode.Toggle, toolTip = "Creates water splashes on everyone’s hands." },
                new() { buttonText = "Water Splash All", method = Fun.WaterSplashAll, mode = ButtonMode.Toggle, toolTip = "Makes other players create water splashes on their hands." },
                new() { buttonText = "Max Quest Score", method =() => SetScore(99999), mode = ButtonMode.Action, toolTip = "Sets your Quest score to the maximum value." },
                new() { buttonText = "Reset Quest Score", method =() => SetScore(0), mode = ButtonMode.Action, toolTip = "Sets your Quest score to 0." },
                new() { buttonText = "Lowercase Name <color=grey>[</color><color=green>Stump</color><color=grey>]</color>", method =() => ChangeName(GorillaComputer.instance.currentName.ToLower()), mode = ButtonMode.Action, toolTip = "Changes your name to lowercase."},
                new() { buttonText = "Rainbow Color <color=grey>[</color><color=green>Stump</color><color=grey>]</color>", method = Fun.RainbowColor, mode = ButtonMode.Toggle, toolTip = "Makes you rainbow."},
                new() { buttonText = "Strobe Color <color=grey>[</color><color=green>Stump</color><color=grey>]</color>", method = Fun.StrobeColor, mode = ButtonMode.Toggle, toolTip = "Makes your color strobe through random colors. [EPILEPSY WARNING]"},
                new() { buttonText = "Hard Rainbow Color <color=grey>[</color><color=green>Stump</color><color=grey>]</color>", method = Fun.HardRainbowColor, mode = ButtonMode.Toggle, toolTip = "Makes you rainbow with no transition."}
        ];

        public static ButtonInfo[] AdminMods =
        [
            new() { buttonText = "Exit Admin Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main menu page." },
                new() { buttonText = "Spawn External Cube", method =() => Admin.CreateExternalObject(PrimitiveType.Cube, Color.white), mode = ButtonMode.Action, toolTip = "Spawns an External cube." },
                new() { buttonText = "Spawn External Sphere", method =() => Admin.CreateExternalObject(PrimitiveType.Sphere, Color.white), mode = ButtonMode.Action, toolTip = "Spawns an External sphere." },
                new() { buttonText = "Spawn External Cube Gun", method =() => Admin.CreateExternalObjectGun(PrimitiveType.Cube, Color.white), mode = ButtonMode.Toggle, toolTip = "Spawns an External cube wherever your hand points." },
                new() { buttonText = "Spawn External Sphere Gun", method =() => Admin.CreateExternalObjectGun(PrimitiveType.Sphere, Color.white), mode = ButtonMode.Toggle, toolTip = "Spawns an External sphere wherever your hand points." },
                new() { buttonText = "Admin Destroy All Networked Objects <color=grey>[</color><color=green>CS</color><color=grey>]</color>", method = ExternalObject.DestroyAll, mode = ButtonMode.Action, toolTip = "Destroys all networked objects, but only for you. [ADMIN]" },
                new() { buttonText = "Admin Destroy All Networked Objects <color=grey>[</color><color=green>SS</color><color=grey>]</color>", method = ExternalObject.DestroyAllNetworked, mode = ButtonMode.Action, toolTip = "Destroys all networked objects. [ADMIN]" },
                new() { buttonText = "Admin Kick Gun", method = Admin.AdminKickGun, mode = ButtonMode.Toggle, toolTip = "Kicks the player you target that is using the menu. [ADMIN]" },
                new() { buttonText = "Admin Kick All", method = Admin.AdminKickAll, mode = ButtonMode.Action, toolTip = "Kicks everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Crash Gun", method = Admin.AdminCrashGun, mode = ButtonMode.Toggle, toolTip = "Crashes the player you target that is using the menu. [ADMIN]" },
                new() { buttonText = "Admin Crash All", method = Admin.AdminCrashAll, mode = ButtonMode.Action, toolTip = "Crashes everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Lag Gun", method = Admin.AdminLagGun, mode = ButtonMode.Toggle, toolTip = "Lags the player you target that is using the menu. [ADMIN]" },
                new() { buttonText = "Admin Lag All", method = Admin.AdminLagAll, mode = ButtonMode.Toggle, toolTip = "Lags everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Close Lobby Kick", method = Admin.AdminKickAll, mode = ButtonMode.Toggle, toolTip = "Closes the lobby by kicking everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Close Lobby Crash", method = Admin.AdminCrashAll, mode = ButtonMode.Toggle, toolTip = "Closes the lobby by crashing everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Annoy Lobby Lag", method = Admin.AdminLagAll, mode = ButtonMode.Toggle, toolTip = "Annoys the lobby by lagging everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Annoy Lobby Sound", method = Admin.AdminAnnoyLobby, mode = ButtonMode.Toggle, toolTip = "Annoys the lobby by spamming sounds for everyone using the menu. [ADMIN]" },
                new() { buttonText = "Admin Annoy Sound Gun", method = Admin.AdminAnnoyGun, mode = ButtonMode.Toggle, toolTip = "Annoys the player of your choosing by spamming sounds on their client. [ADMIN]" },
                new() { buttonText = "Admin Lag Spike Gun [CAN CRASH]", method = Admin.AdminLagSpikeGun, mode = ButtonMode.Toggle, toolTip = "Gives the player you target severe lag spikes. [ADMIN]" },
                new() { buttonText = "Admin Lag Spike All [CAN CRASH]", method = Admin.AdminLagSpikeAll, mode = ButtonMode.Toggle, toolTip = "Gives everyone using the menu severe lag spikes. [ADMIN]" },
                new() { buttonText = "Admin Bring All", method = Admin.BringAll, mode = ButtonMode.Action, toolTip = "Brings everyone to your location. [ADMIN]" },
                new() { buttonText = "Admin Follow All", method = Admin.BringAll, mode = ButtonMode.Toggle, toolTip = "Brings everyone to your location constantly. [ADMIN]" },
                new() { buttonText = "Admin Bring Gun", method = Admin.BringGun, mode = ButtonMode.Toggle, toolTip = "Brings the player you target to your location. [ADMIN]" },
                new() { buttonText = "Admin Hide Gun", method = Admin.AdminHideGun, mode = ButtonMode.Toggle, toolTip = "Hides the player you target. [ADMIN]" },
                new() { buttonText = "Admin Hide All", method = Admin.AdminHideAll, mode = ButtonMode.Action, toolTip = "Hides all players. [ADMIN]" },
                new() { buttonText = "Admin Unhide All", method = Admin.UnhideAll, mode = ButtonMode.Action, toolTip = "Unhides all players. [ADMIN]" },
                new() { buttonText = "Admin Ban Gun [NAME BAN]", method = Admin.BanGun, mode = ButtonMode.Toggle, toolTip = "Bans the player you target using the menu for 1 hour.  [ADMIN]" },
            ];

        public static ButtonInfo[] EnabledMods =
        [
            new() { buttonText = "Exit Enabled Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true, toolTip = "Returns to the main menu page." }
        ];

        public static ButtonCategory[] Categories =
            [
                new() { name = "Main", buttons = MainMods },
                new() { name = "Settings", buttons = Settings },
                new() { name = "Menu Settings", buttons = MenuSettings },
                new() { name = "Movement Settings", buttons = MovementSettings },
                new() { name = "Room Mods", buttons = RoomMods },
                new() { name = "Queue Room Mods", buttons = QueueRoomMods},
                new() { name = "Movement Mods", buttons = MovementMods },
                new() { name = "Safety Mods", buttons = SafetyMods },
                new() { name = "Visual Mods", buttons = VisualMods },
                new() { name = "Player Mods", buttons = PlayerMods },
                new() { name = "Advantage Mods", buttons = AdvantageMods },
                new() { name = "Fun Mods", buttons = FunMods },
                new() { name = "Overpowered Mods", buttons = OpMods},
                new() { name = "Admin", buttons = AdminMods},
                new() { name = "VIM", buttons = VIMMods},
                new() { name = "Important Mods", buttons = ImportantMods },
                new() { name = "Enabled Mods", buttons = EnabledMods },
            ];


        private static readonly Dictionary<string, ButtonCategory> CategoryLookup = BuildCategoryLookup();
        private static readonly Dictionary<string, ButtonInfo> ButtonLookup = BuildButtonLookup();

        private static readonly Dictionary<string, ButtonInfo> SavedToggleButtonLookup =
                        BuildSavedToggleButtonLookup();

        public static IReadOnlyDictionary<string, ButtonInfo> SavedToggleButtons =>
                        SavedToggleButtonLookup;

        public static ButtonInfo[] AllButtons { get; } = BuildButtonArray();

        public static int ButtonCount =>
            AllButtons.Count(button => button != null && !button.isCategory);

        public static ButtonCategory GetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return null;

            CategoryLookup.TryGetValue(categoryName, out ButtonCategory category);

            return category;
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            if (string.IsNullOrWhiteSpace(buttonText))
                return null;

            ButtonLookup.TryGetValue(buttonText, out ButtonInfo button);

            return button;
        }

        private static Dictionary<string, ButtonCategory> BuildCategoryLookup()
        {
            Dictionary<string, ButtonCategory> lookup = new(StringComparer.OrdinalIgnoreCase);

            foreach (ButtonCategory category in Categories)
            {
                if (string.IsNullOrWhiteSpace(category.name))
                    throw new InvalidOperationException("A button category does not have a name.");

                if (!lookup.TryAdd(category.name, category))
                    throw new InvalidOperationException($"Duplicate button category named {category.name}.");

            }

            return lookup;
        }

        private static Dictionary<string, ButtonInfo> BuildButtonLookup()
        {
            Dictionary<string, ButtonInfo> lookup = new(StringComparer.OrdinalIgnoreCase);

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    if (string.IsNullOrWhiteSpace(button.buttonText))
                        continue;

                    // Duplicate names such as "Return to Main" are allowed.
                    // Actual menu presses reference their ButtonInfo directly,
                    // so this lookup is only used when something explicitly requests a button by name.
                    lookup.TryAdd(button.buttonText, button);
                }
            }

            return lookup;
        }

        private static ButtonInfo[] BuildButtonArray()
        {
            int buttonCount = Categories.Sum(category => category.buttons.Length);

            ButtonInfo[] result = new ButtonInfo[buttonCount];

            int index = 0;

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    result[index] = button;
                    index++;
                }
            }

            return result;
        }

        private static Dictionary<string, ButtonInfo> BuildSavedToggleButtonLookup()
        {
            Dictionary<string, ButtonInfo> result =
                            new(
                                            StringComparer.Ordinal);

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    if (button.mode != ButtonMode.Toggle)
                        continue;

                    string key =
                                    category.name +
                                    "/" +
                                    button.buttonText;

                    if (!result.TryAdd(key, button))
                    {
                        throw new InvalidOperationException(
                                        $"Duplicate saved toggle button key {key}.");
                    }

                }
            }

            return result;
        }
    }
}