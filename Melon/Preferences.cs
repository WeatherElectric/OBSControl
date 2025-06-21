// ReSharper disable MemberCanBePrivate.Global, these categories may be used outside of this namespace to create bonemenu options.

using MelonLoader.Utils;

namespace WeatherElectric.OBSControl.Melon;

internal static class Preferences
{
    public static readonly MelonPreferences_Category GlobalCategory = MelonPreferences.CreateCategory("Global");
    public static readonly MelonPreferences_Category OwnCategory = MelonPreferences.CreateCategory("OBSControl");

    public static MelonPreferences_Entry<int> LoggingMode { get; set; }
    public static MelonPreferences_Entry<bool> ShowNotifications { get; set; }
    public static MelonPreferences_Entry<ControlHand> ReplayControlHand { get; set; }
    public static MelonPreferences_Entry<float> DoubleTapTime { get; set; }

    public static void Setup()
    {
        LoggingMode = GlobalCategory.GetEntry<int>("LoggingMode") ?? GlobalCategory.CreateEntry("LoggingMode", 0,
            "Logging Mode", "The level of logging to use. 0 = Important Only, 1 = All");
        GlobalCategory.SetFilePath(MelonEnvironment.UserDataDirectory + "/WeatherElectric.cfg");
        GlobalCategory.SaveToFile(false);
        
        ShowNotifications = OwnCategory.CreateEntry("ShowNotifications", true, "Show Notifications", "Whether to show notifications when OBS events occur.");
        ReplayControlHand = OwnCategory.CreateEntry("ReplayControlHand", ControlHand.Right, "Replay Control Hand", "The hand to use for saving replays. Left = Left hand, Right = Right hand, Both = Both hands.");
        DoubleTapTime = OwnCategory.CreateEntry("DoubleTapTime", 0.3f, "Double Tap Time", "The time to wait between taps to trigger saving a replay.");
        OwnCategory.SetFilePath(MelonEnvironment.UserDataDirectory + "/WeatherElectric.cfg");
        OwnCategory.SaveToFile(false);
        ModConsole.Msg("Finished preferences setup for OBSControl", 1);
    }
}

internal enum ControlHand
{
    Left,
    Right,
    Both
}