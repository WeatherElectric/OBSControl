using WeatherElectric.OBSControl.Handlers;

namespace WeatherElectric.OBSControl;

internal static class ControlHandler
{
    private static bool _isFirstTap;
    private static float _doubleTapTimer;
    
    public static void Update()
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
            if (Player.RightController._menuTap || Player.LeftController._menuTap)
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
                        NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                        _isFirstTap = false;
                    }
                }
            }
        }

        void HandleLeft()
        {
            if (Player.LeftController._menuTap)
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
                        NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                        _isFirstTap = false;
                    }
                }
            }
        }

        void HandleRight()
        {
            if (Player.RightController._menuTap)
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
                        NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                        _isFirstTap = false;
                    }
                }
            }
        }
    }
}