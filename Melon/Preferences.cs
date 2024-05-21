// ReSharper disable MemberCanBePrivate.Global, these categories may be used outside of this namespace to create bonemenu options.

namespace WeatherElectric.OBSControl.Melon;

internal static class Preferences
{
    public static readonly MelonPreferences_Category GlobalCategory = MelonPreferences.CreateCategory("Global");
    public static readonly MelonPreferences_Category OwnCategory = MelonPreferences.CreateCategory("OBSControl");

    public static MelonPreferences_Entry<int> LoggingMode { get; set; }

    public static void Setup()
    {
        LoggingMode = GlobalCategory.GetEntry<int>("LoggingMode") ?? GlobalCategory.CreateEntry("LoggingMode", 0,
            "Logging Mode", "The level of logging to use. 0 = Important Only, 1 = All");
        GlobalCategory.SetFilePath(MelonUtils.UserDataDirectory + "/WeatherElectric.cfg");
        GlobalCategory.SaveToFile(false);
        OwnCategory.SetFilePath(MelonUtils.UserDataDirectory + "/WeatherElectric.cfg");
        OwnCategory.SaveToFile(false);
        ModConsole.Msg("Finished preferences setup for OBSControl", 1);
    }
}