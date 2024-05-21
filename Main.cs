namespace WeatherElectric.OBSControl;

public class Main : MelonMod
{
    internal const string Name = "OBSControl";
    internal const string Description = "Control OBS from within BONELAB.";
    internal const string Author = "SoulWithMae";
    internal const string Company = "Weather Electric";
    internal const string Version = "0.0.1";
    internal const string DownloadLink = null;
    
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        ObsBridge.Connect();
        Hooking.OnLevelInitialized += OnLevelLoaded;
        Hooking.OnLevelUnloaded += OnLevelUnloaded;
    }
    
    public override void OnLateInitializeMelon()
    {
        // i want this to be called late to give OBS time to connect, since the bonemenu setup method uses OBS
        BoneMenu.Setup();
    }
    
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
    
    public override void OnUpdate()
    {
        if (!_rigExists) return;
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
        switch (Preferences.ReplayControlHand.Value)
        {
            case ControlHand.Left:
                HandleLeft();
                break;
            case ControlHand.Right:
                HandleRight();
                break;
            case ControlHand.Both:
                HandleBoth();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        if (_isFirstTap)
        {
            _doubleTapTimer += Time.deltaTime;

            if (_doubleTapTimer > DoubleTapTime)
            {
                _isFirstTap = false;
            }
        }

        return;

        void HandleBoth()
        {
            if (Player.rightController._touchPad || Player.leftController._touchPad)
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
        }

        void HandleLeft()
        {
            if (Player.leftController._touchPad)
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
        }

        void HandleRight()
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
        }
    }
    
    private void MenuButton()
    {
        switch (Preferences.ReplayControlHand.Value)
        {
            case ControlHand.Left:
                HandleLeft();
                break;
            case ControlHand.Right:
                HandleRight();
                break;
            case ControlHand.Both:
                HandleBoth();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (_isFirstTap)
        {
            _doubleTapTimer += Time.deltaTime;

            if (_doubleTapTimer > DoubleTapTime)
            {
                _isFirstTap = false;
            }
        }
        
        return;

        void HandleBoth()
        {
            if (Player.rightController._menuTap || Player.leftController._menuTap)
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
        }

        void HandleLeft()
        {
            if (Player.leftController._menuTap)
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
        }

        void HandleRight()
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
        }
    }
}