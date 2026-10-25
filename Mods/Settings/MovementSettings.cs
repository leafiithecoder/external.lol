using SharpzReborn.Classes;
using SharpzReborn.Notifications;
using UnityEngine;
using static SharpzReborn.Menu.Buttons;

namespace SharpzReborn.Mods.Settings;

// asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf
public class Movement
{
    public static readonly string[] flySpeedNames =
    [
        "Very Slow",
        "Slow",
        "Normal",
        "Fast",
        "Very Fast",
        "Extreme"
    ];

    public static readonly float[] flySpeedValues =
    [
        5f,
        10f,
        15f,
        20f,
        30f,
        50f
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedFlySpeed))]
    public static int flySpeedIndex = 2;

    public static float flySpeed = 15f;

    public static string FlySpeedName => flySpeedNames[flySpeedIndex];

    public static void ChangeFlySpeed(bool increment)
    {
        flySpeedIndex += increment ? 1 : -1;

        if (flySpeedIndex >= flySpeedNames.Length)
            flySpeedIndex = 0;
        else if (flySpeedIndex < 0)
            flySpeedIndex = flySpeedNames.Length - 1;

        ApplySavedFlySpeed();
    }

    public static void SetFlySpeedIndex(int index)
    {
        flySpeedIndex = Mathf.Clamp(index, 0, flySpeedValues.Length - 1);
        ApplySavedFlySpeed();
    }

    private static void ApplySavedFlySpeed()
    {
        flySpeedIndex = Mathf.Clamp(flySpeedIndex, 0, flySpeedValues.Length - 1);
        flySpeed = flySpeedValues[flySpeedIndex];
        GetIndex("Change Fly Speed").overlapText = $"Change Fly Speed <color=grey>[</color><color=green>{flySpeedNames[flySpeedIndex]}</color><color=grey>]</color>";
    }

    public static int speedAmountIndex = 1;
    public static float speedAmount = 7.5f;
    public static float speedMultiplier;

    public static readonly string[] speedNames =
    [
        "Unnoticeable",
        "Normal",
        "Medium",
        "Fast",
        "Ultra Fast"
    ];

    public static readonly float[] speedValues =
    [
        6.5f,
        7.7f,
        8.5f,
        9f,
        20f
    ];

    public static readonly float[] speedMultis =
    [
        4.1f,
        5f,
        5.5f,
        6.5f,
        10f
    ];

    public static string SpeedAmountName => speedNames[speedAmountIndex];

    public static void ChangeSpeedBoostAmount(bool increment)
    {
        speedAmountIndex += increment ? 1 : -1;

        if (speedAmountIndex >= speedNames.Length)
            speedAmountIndex = 0;
        else if (speedAmountIndex < 0)
            speedAmountIndex = speedNames.Length - 1;

        ApplySavedSpeedAmount();
    }

    public static void SetSpeedAmountIndex(int index)
    {
        speedAmountIndex = Mathf.Clamp(index, 0, speedValues.Length - 1);
        ApplySavedSpeedAmount();
    }

    private static void ApplySavedSpeedAmount()
    {
        speedAmountIndex = Mathf.Clamp(speedAmountIndex, 0, speedValues.Length - 1);
        speedAmount = speedValues[speedAmountIndex];
        speedMultiplier = speedMultis[speedAmountIndex];
        GetIndex("Change Speed Boost Amount").overlapText = $"Change Speed Boost Amount <color=grey>[</color><color=green>{speedNames[speedAmountIndex]}</color><color=grey>]</color>";
    }
    public static readonly float[] armLengthAmounts =
    [
    0.75f,
    1.1f,
    1.25f,
    1.5f,
    2f
    ];

    public static readonly string[] armLengthNames =
    [
    "Short",
    "Unnoticeable",
    "Normal",
    "Long",
    "Extreme"
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedArmLength))]
    public static int armLengthIndex = 2;

    public static float armLength = 1.25f;

    public static string ArmLengthName => armLengthNames[armLengthIndex];

    public static void ChangeArmLength(bool increment)
    {
        armLengthIndex += increment ? 1 : -1;

        if (armLengthIndex >= armLengthNames.Length)
            armLengthIndex = 0;
        else if (armLengthIndex < 0)
            armLengthIndex = armLengthNames.Length - 1;

        ApplySavedArmLength();
    }

    public static void SetArmLengthIndex(int index)
    {
        armLengthIndex = Mathf.Clamp(index, 0, armLengthAmounts.Length - 1);
        ApplySavedArmLength();
    }

    private static void ApplySavedArmLength()
    {
        armLengthIndex = Mathf.Clamp(armLengthIndex, 0, armLengthAmounts.Length - 1);
        armLength = armLengthAmounts[armLengthIndex];
        GetIndex("Change Arm Length").overlapText = $"Change Arm Length [{armLengthNames[armLengthIndex]}]";
    }

    public static readonly PrimitiveType[] platformShapes =
[
    PrimitiveType.Cube,
    PrimitiveType.Sphere
    ];

    public static readonly string[] platformShapeNames =
    [
        "Cube",
        "Sphere"
    ];

    public static readonly Vector3[] platformShapeScales =
    [
        new Vector3(0.025f, 0.3f, 0.4f),
        new Vector3(0.444f, 0.444f, 0.444f)
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedPlatformShape))]
    public static int platformShapeIndex = 0;

    public static PrimitiveType platformShape = PrimitiveType.Cube;

    public static Vector3 PlatformShapeScale =>
        platformShapeScales[platformShapeIndex];

    public static string PlatformShapeName =>
        platformShapeNames[platformShapeIndex];

    public static void ChangePlatformShape(bool increment)
    {
        platformShapeIndex += increment ? 1 : -1;

        if (platformShapeIndex >= platformShapeNames.Length)
            platformShapeIndex = 0;
        else if (platformShapeIndex < 0)
            platformShapeIndex = platformShapeNames.Length - 1;

        ApplySavedPlatformShape();
    }

    public static void SetPlatformShapeIndex(int index)
    {
        platformShapeIndex =
            Mathf.Clamp(index, 0, platformShapes.Length - 1);

        ApplySavedPlatformShape();
    }

    private static void ApplySavedPlatformShape()
    {
        platformShapeIndex =
            Mathf.Clamp(
                platformShapeIndex,
                0,
                platformShapes.Length - 1
            );

        platformShape =
            platformShapes[platformShapeIndex];

        GetIndex("Change Platform Shape").overlapText =
            $"Change Platform Shape [{platformShapeNames[platformShapeIndex]}]";
    }
}
