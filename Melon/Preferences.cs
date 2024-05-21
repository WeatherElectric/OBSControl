// ReSharper disable MemberCanBePrivate.Global, these categories may be used outside of this namespace to create bonemenu options.

namespace WeatherElectric.OBSControl.Melon;

internal static class Preferences
{
    public static readonly MelonPreferences_Category GlobalCategory = MelonPreferences.CreateCategory("Global");
    public static readonly MelonPreferences_Category OwnCategory = MelonPreferences.CreateCategory("OBSControl");

    public static MelonPreferences_Entry<int> LoggingMode { get; set; }
    public static MelonPreferences_Entry<bool> ShowNotifications { get; set; }
    public static MelonPreferences_Entry<string> WebsocketURL { get; set; }
    public static MelonPreferences_Entry<string> WebsocketPassword { get; set; }
    public static MelonPreferences_Entry<ControlMode> ReplayControlMode { get; set; }

    public static void Setup()
    {
        LoggingMode = GlobalCategory.GetEntry<int>("LoggingMode") ?? GlobalCategory.CreateEntry("LoggingMode", 0,
            "Logging Mode", "The level of logging to use. 0 = Important Only, 1 = All");
        GlobalCategory.SetFilePath(MelonUtils.UserDataDirectory + "/WeatherElectric.cfg");
        GlobalCategory.SaveToFile(false);
        
        ShowNotifications = OwnCategory.CreateEntry("ShowNotifications", true, "Show Notifications", "Whether to show notifications when OBS events occur.");
        WebsocketURL = OwnCategory.CreateEntry("WebsocketURL", "ws://127.0.0.1:4455", "Websocket URL", "The URL to use for the websocket connection. Usually don't have to change this.");
        WebsocketPassword = OwnCategory.CreateEntry("WebsocketPassword", "REPLACEME", "Websocket Password", "The password to use for the websocket connection. Change this to the password you set in OBS.");
        ReplayControlMode = OwnCategory.CreateEntry("ReplayControlMode", ControlMode.Touchpad, "Replay Control Mode", "The mode to use for saving replays. Touchpad = Use the touchpad, MenuButton = Use the menu button.");
        OwnCategory.SetFilePath(MelonUtils.UserDataDirectory + "/WeatherElectric.cfg");
        OwnCategory.SaveToFile(false);
        ModConsole.Msg("Finished preferences setup for OBSControl", 1);
    }
}

internal enum ControlMode
{
    Touchpad,
    MenuButton
}