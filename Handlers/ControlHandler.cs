namespace WeatherElectric.OBSControl;

internal static class ControlHandler
{
    private static bool _isFirstTap;
    private static float _doubleTapTimer;
    
    public static void Update()
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
    
    private static void Touchpad()
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

            if (_doubleTapTimer > Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
                    {
                        ObsBridge.SaveReplayBuffer();
                        _isFirstTap = false;
                    }
                }
            }
        }
    }
    
    private static void MenuButton()
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

            if (_doubleTapTimer > Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
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
                    if (_doubleTapTimer < Preferences.DoubleTapTime.Value)
                    {
                        ObsBridge.SaveReplayBuffer();
                        _isFirstTap = false;
                    }
                }
            }
        }
    }
}