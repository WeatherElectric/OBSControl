namespace WeatherElectric.OBSControl;

/// <inheritdoc />
public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "2.0.0";
    internal const string DownloadLink = "https://thunderstore.io/c/bonelab/p/SoulWithMae/OBSControl/";
    
    private static bool _rigExists;

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
#if DEBUG
        ModConsole.Log("This is a debug build!");
#endif
        ObsBridge.Connect();
        ObsBridge.InitHooks();
        Hooking.OnUIRigCreated += OnUIRigCreated;
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
    
    /// <inheritdoc />
    public override void OnUpdate()
    {
        if (!_rigExists) return;
        ControlHandler.Update();
    }

    private static void OnUIRigCreated()
    {
        _rigExists = true;
    }

    private static void OnLevelUnloaded()
    {
        _rigExists = false;
    }
}