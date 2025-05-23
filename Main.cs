﻿namespace WeatherElectric.OBSControl;

/// <inheritdoc />
public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "Mabel Amber";
    internal const string Company = "Weather Electric";
    internal const string Version = "1.2.0";
    internal const string DownloadLink = "https://thunderstore.io/c/bonelab/p/SoulWithMae/OBSControl/";
    
    private static bool _rigExists;

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
#if DEBUG
        ModConsole.Msg("This is a debug build!");
#endif
        
        Hooking.OnUIRigCreated += OnUIRigCreated;
        Hooking.OnLevelUnloaded += OnLevelUnloaded;
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
        if (!ObsBridge.Connected) return;
        ControlHandler.Update();
    }

    private static void OnUIRigCreated()
    {
        _rigExists = true;
        
        if (ObsBridge.Connected) return;
        ObsBridge.Connect();
        ObsBridge.InitHooks();
    }

    private static void OnLevelUnloaded()
    {
        _rigExists = false;
    }
}