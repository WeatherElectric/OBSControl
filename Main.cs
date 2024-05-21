namespace WeatherElectric.OBSControl;

public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "0.0.1";
    internal const string DownloadLink = null;
    
    internal static Assembly ModAsm => Assembly.GetExecutingAssembly();

    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        UserData.Setup();
        ObsBridge.Connect();
    }
    
    public override void OnLateInitializeMelon()
    {
        // i want this to be called late to give OBS time to connect, since the bonemenu setup method uses OBS
        BoneMenu.Setup();
    }
    
    public override void OnUpdate()
    {
        switch (Preferences.ReplayControlMode.Value)
        {
            case ControlMode.Touchpad:
                Touchpad();
                break;
            case ControlMode.MenuButton:
                MenuButton();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private static bool _isFirstTap;
    private float _doubleTapTimer;
    private const float DoubleTapTime = 0.3f;
    
    private void Touchpad()
    {
        if (Player.rightController._touchPad)
        {
            if (!_isFirstTap)
            {
                _isFirstTap = true;
                _doubleTapTimer = 0;
            }
            else
            {
                if (_doubleTapTimer < DoubleTapTime)
                {
                    ObsBridge.SaveReplayBuffer();
                    _isFirstTap = false;
                }
            }
        }
        
        if (_isFirstTap)
        {
            _doubleTapTimer += Time.deltaTime;

            if (_doubleTapTimer > DoubleTapTime)
            {
                _isFirstTap = false;
            }
        }
    }
    
    private void MenuButton()
    {
        if (Player.rightController._menuTap)
        {
            if (!_isFirstTap)
            {
                _isFirstTap = true;
                _doubleTapTimer = 0;
            }
            else
            {
                if (_doubleTapTimer < DoubleTapTime)
                {
                    ObsBridge.SaveReplayBuffer();
                    _isFirstTap = false;
                }
            }
        }
        
        if (_isFirstTap)
        {
            _doubleTapTimer += Time.deltaTime;

            if (_doubleTapTimer > DoubleTapTime)
            {
                _isFirstTap = false;
            }
        }
    }
}