namespace WeatherElectric.OBSControl.Menu;

internal static class BoneMenu
{
    private static FunctionElement _recordButton;
    private static FunctionElement _pauseButton;
    private static FunctionElement _streamButton;
    private static FunctionElement _replayButton;
    
    public static void Setup()
    {
        MenuCategory mainCat = MenuManager.CreateCategory("Weather Electric", "#6FBDFF");
        MenuCategory subCat = mainCat.CreateCategory("OBSControl", Color.blue);
        
        #region Recording
        
        SubPanelElement recordPanel = subCat.CreateSubPanel("Record", Color.green);
        _recordButton = recordPanel.CreateFunctionElement("Record Button", Color.green, () =>
        {
            switch (ObsBridge.IsRecording())
            {
                case true:
                    ObsBridge.StopRecording();
                    SetRecordButton(false);
                    Notifications.SendNotif(Notifications.RecordingStopped);
                    break;
                case false:
                    ObsBridge.StartRecording();
                    SetRecordButton(true);
                    Notifications.SendNotif(Notifications.RecordingStarted);
                    break;
            }
        });
        SetRecordButton(ObsBridge.IsRecording());
        _pauseButton = recordPanel.CreateFunctionElement("Pause Button", Color.yellow, () =>
        {
            switch (ObsBridge.IsRecordingPaused())
            {
                case true:
                    ObsBridge.ResumeRecording();
                    SetPauseButton(false);
                    Notifications.SendNotif(Notifications.RecordingResumed);
                    break;
                case false:
                    ObsBridge.PauseRecording();
                    SetPauseButton(true);
                    Notifications.SendNotif(Notifications.RecordingPaused);
                    break;
            }
        });
        SetPauseButton(ObsBridge.IsRecordingPaused());
        
        #endregion
        
        #region Streaming
        
        SubPanelElement streamPanel = subCat.CreateSubPanel("Stream", Color.blue);
        _streamButton = streamPanel.CreateFunctionElement("Stream Button", Color.blue, () =>
        {
            switch (ObsBridge.IsStreaming())
            {
                case true:
                    ObsBridge.StopStreaming();
                    SetStreamButton(false);
                    Notifications.SendNotif(Notifications.StreamStopped);
                    break;
                case false:
                    ObsBridge.StartStreaming();
                    SetStreamButton(true);
                    Notifications.SendNotif(Notifications.StreamStarted);
                    break;
            }
        });
        SetStreamButton(ObsBridge.IsStreaming());
        
        #endregion
        
        #region Replay
        
        SubPanelElement replayPanel = subCat.CreateSubPanel("Replay", Color.yellow);
        _replayButton = replayPanel.CreateFunctionElement("Replay Button", Color.blue, () =>
        {
            switch (ObsBridge.IsReplayBufferActive())
            {
                case true:
                    ObsBridge.StopReplayBuffer();
                    SetReplayButton(false);
                    Notifications.SendNotif(Notifications.ReplayBufferStopped);
                    break;
                case false:
                    ObsBridge.StartReplayBuffer();
                    SetReplayButton(true);
                    Notifications.SendNotif(Notifications.ReplayBufferStarted);
                    break;
            }
        });
        SetReplayButton(ObsBridge.IsReplayBufferActive());
        replayPanel.CreateFunctionElement("Save Replay", Color.blue, () =>
        {
            ObsBridge.SaveReplayBuffer();
            Notifications.SendNotif(Notifications.ReplaySaved);
        });
        
        #endregion
        
        #region Scenes
        
        SubPanelElement scenesPanel = subCat.CreateSubPanel("Scenes", Color.red);
        var scenes = ObsBridge.GetScenes();
        foreach (var scene in scenes)
        {
            scenesPanel.CreateFunctionElement(scene.Name, Color.white, () =>
            {
                ObsBridge.SetScene(scene.Name);
            });
        }
        
        #endregion
        
        #region Settings
        
        SubPanelElement settingsPanel = subCat.CreateSubPanel("Settings", Color.gray);
        settingsPanel.CreateBoolPreference("Show Notifications", Color.white, Preferences.ShowNotifications, Preferences.OwnCategory);
        settingsPanel.CreateEnumPreference("Replay Control Mode", Color.white, Preferences.ReplayControlMode, Preferences.OwnCategory);
        settingsPanel.CreateEnumPreference("Replay Control Hand", Color.white, Preferences.ReplayControlHand, Preferences.OwnCategory);
        
        #endregion
    }
    
    private static void SetRecordButton(bool isRecording)
    {
        if (isRecording)
        {
            _recordButton.SetColor(Color.red);
            _recordButton.SetName("Stop Recording");
        }
        else
        {
            _recordButton.SetColor(Color.green);
            _recordButton.SetName("Start Recording");
        }
    }
    
    private static void SetPauseButton(bool isPaused)
    {
        if (isPaused)
        {
            _pauseButton.SetColor(Color.white);
            _pauseButton.SetName("Resume Recording");
        }
        else
        {
            _pauseButton.SetColor(Color.yellow);
            _pauseButton.SetName("Pause Recording");
        }
    }

    private static void SetStreamButton(bool isStreaming)
    {
        if (isStreaming)
        {
            _streamButton.SetColor(Color.red);
            _streamButton.SetName("Stop Streaming");
        }
        else
        {
            _streamButton.SetColor(Color.green);
            _streamButton.SetName("Start Streaming");
        }
    }
    
    private static void SetReplayButton(bool isReplayActive)
    {
        if (isReplayActive)
        {
            _replayButton.SetColor(Color.red);
            _replayButton.SetName("Stop Replay Buffer");
        }
        else
        {
            _replayButton.SetColor(Color.green);
            _replayButton.SetName("Start Replay Buffer");
        }
    }
}