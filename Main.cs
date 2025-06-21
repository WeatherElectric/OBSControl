namespace WeatherElectric.OBSControl;

public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "Mabel Amber";
    internal const string Company = "Weather Electric";
    internal const string Version = "2.0.0";
    internal const string DownloadLink = "https://thunderstore.io/c/bonelab/p/SoulWithMae/OBSControl/";
    
    private static bool _rigExists;
    
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
#if DEBUG
        ModConsole.Msg("This is a debug build!");
#endif

        ObsBridge.OnConnected += OnOBSConnected;
        Hooking.OnLevelLoaded += OnLevelLoaded;
        Hooking.OnLevelUnloaded += OnLevelUnloaded;
        BoneMenu.SetupBaseMenu();
    }

    private static void OnOBSConnected(object sender, EventArgs e)
    {
        BoneMenu.SetupObsControls();
    }
    
    public override void OnUpdate()
    {
        if (!_rigExists) return;
        if (!ObsBridge.Connected) return;
        ControlHandler.Update();
    }

    private static void OnLevelLoaded(LevelInfo levelInfo)
    {
        _rigExists = true;
    }

    private static void OnLevelUnloaded()
    {
        _rigExists = false;
    }
}