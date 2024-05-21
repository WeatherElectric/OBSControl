namespace WeatherElectric.OBSControl;

/// <inheritdoc />
public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "0.0.1";
    internal const string DownloadLink = null;

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        ObsBridge.Connect();
        ObsBridge.InitHooks();
        Hooking.OnLevelInitialized += OnLevelLoaded;
        Hooking.OnLevelUnloaded += OnLevelUnloaded;
    }

    /// <inheritdoc />
    public override void OnLateInitializeMelon()
    {
        // i want this to be called late to give OBS time to connect, since the bonemenu setup method uses OBS
        BoneMenu.SetupBaseMenu();
    }

    /// <inheritdoc />
    public override void OnApplicationQuit()
    {
        ObsBridge.Disconnect();
    }

    private static bool _rigExists;

    private static void OnLevelLoaded(LevelInfo levelInfo)
    {
        _rigExists = true;
    }

    private static void OnLevelUnloaded()
    {
        _rigExists = false;
    }

    /// <inheritdoc />
    public override void OnUpdate()
    {
        if (!_rigExists) return;
        ControlHandler.Update();
    }
}