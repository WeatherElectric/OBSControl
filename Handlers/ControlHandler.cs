using WeatherElectric.OBSControl.Handlers;

namespace WeatherElectric.OBSControl;

internal static class ControlHandler
{
    private static bool _isFirstTap;
    private static float _doubleTapTimer;
    
    public static void Update()
    {
        if (!ObsBridge.Connected) return;
        if (!ObsBridge.ReplayBufferActive) return;
        
#if DEBUG
        ModConsole.Msg($"ReplayControlHand: {Preferences.ReplayControlHand.Value}");
#endif
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

        if (!_isFirstTap) return;
        _doubleTapTimer += Time.deltaTime;

        if (_doubleTapTimer > Preferences.DoubleTapTime.Value)
        {
            _isFirstTap = false;
        }

        return;

        void HandleBoth()
        {
#if DEBUG
            ModConsole.Msg("Handling both");
#endif
            if (!Player.RightController._menuTap && !Player.LeftController._menuTap) return;
            if (!_isFirstTap)
            {
                _isFirstTap = true;
                _doubleTapTimer = 0;
            }
            else
            {
                if (!(_doubleTapTimer < Preferences.DoubleTapTime.Value)) return;
                ObsBridge.SaveReplayBuffer();
                NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                _isFirstTap = false;
            }
        }

        void HandleLeft()
        {
#if DEBUG
            ModConsole.Msg("Handling left");
#endif
            if (!Player.LeftController._menuTap) return;
            if (!_isFirstTap)
            {
                _isFirstTap = true;
                _doubleTapTimer = 0;
            }
            else
            {
                if (!(_doubleTapTimer < Preferences.DoubleTapTime.Value)) return;
                ObsBridge.SaveReplayBuffer();
                NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                _isFirstTap = false;
            }
        }

        void HandleRight()
        {
#if DEBUG
            ModConsole.Msg("Handling right");
#endif
            if (!Player.RightController._menuTap) return;
            if (!_isFirstTap)
            {
                _isFirstTap = true;
                _doubleTapTimer = 0;
            }
            else
            {
                if (!(_doubleTapTimer < Preferences.DoubleTapTime.Value)) return;
                ObsBridge.SaveReplayBuffer();
                NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
                _isFirstTap = false;
            }
        }
    }
}