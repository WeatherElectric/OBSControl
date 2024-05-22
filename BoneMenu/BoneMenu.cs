using OBSWebsocketDotNet.Types.Events;
using UnityEngine.Diagnostics;
using WeatherElectric.OBSControl.Handlers;

namespace WeatherElectric.OBSControl.Menu;

internal static class BoneMenu
{
    private static MenuCategory _subCat;
    private static FunctionElement _recordButton;
    private static FunctionElement _pauseButton;
    private static FunctionElement _streamButton;
    private static FunctionElement _replayButton;
    
    private static SubPanelElement _scenesPanel;
    private static readonly Dictionary<string, FunctionElement> SceneButtons = [];
    
    
    public static void SetupBaseMenu()
    {
        MenuCategory mainCat = MenuManager.CreateCategory("Weather Electric", "#6FBDFF");
        _subCat = mainCat.CreateCategory("OBSControl", "#284cb8");
        CheckIfConnected();
    }

    private static void CheckIfConnected()
    {
        if (!ObsBridge.IsConnected())
        {
            _subCat.CreateFunctionElement("OBS is not connected! Restart the game with OBS open!", Color.red, () =>
            {
                Utils.ForceCrash(ForcedCrashCategory.Abort);
            }, "This will crash the game intentionally to close the game!");
            return;
        }
        
        SetupObsControls();
    }

    private static void SetupObsControls()
    {
        #region Recording
        
        SubPanelElement recordPanel = _subCat.CreateSubPanel("Record", Color.green);
        _recordButton = recordPanel.CreateFunctionElement("Record Button", Color.green, () =>
        {
            switch (ObsBridge.IsRecording())
            {
                case true:
                    ObsBridge.StopRecording();
                    break;
                case false:
                    ObsBridge.StartRecording();
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
                    break;
                case false:
                    ObsBridge.PauseRecording();
                    break;
            }
        });
        SetPauseButton(ObsBridge.IsRecordingPaused());
        
        #endregion
        
        #region Streaming
        
        SubPanelElement streamPanel = _subCat.CreateSubPanel("Stream", Color.blue);
        _streamButton = streamPanel.CreateFunctionElement("Stream Button", Color.blue, () =>
        {
            switch (ObsBridge.IsStreaming())
            {
                case true:
                    ObsBridge.StopStreaming();
                    break;
                case false:
                    ObsBridge.StartStreaming();
                    break;
            }
        });
        SetStreamButton(ObsBridge.IsStreaming());
        
        #endregion
        
        #region Replay
        
        SubPanelElement replayPanel = _subCat.CreateSubPanel("Replay", Color.yellow);
        _replayButton = replayPanel.CreateFunctionElement("Replay Button", Color.blue, () =>
        {
            switch (ObsBridge.IsReplayBufferActive())
            {
                case true:
                    ObsBridge.StopReplayBuffer();
                    break;
                case false:
                    ObsBridge.StartReplayBuffer();
                    break;
            }
        });
        SetReplayButton(ObsBridge.IsReplayBufferActive());
        replayPanel.CreateFunctionElement("Save Replay", Color.blue, ObsBridge.SaveReplayBuffer);
        
        #endregion
        
        #region Scenes
        
        _scenesPanel = _subCat.CreateSubPanel("Scenes", Color.red);
        var scenes = ObsBridge.GetScenes();
        foreach (var scene in scenes)
        {
            var func= _scenesPanel.CreateFunctionElement(scene.Name, Color.white, () =>
            {
                ObsBridge.SetScene(scene.Name);
            });
            SceneButtons.Add(scene.Name, func);
        }
        
        #endregion
        
        #region Settings
        
        SubPanelElement settingsPanel = _subCat.CreateSubPanel("Settings", Color.gray);
        settingsPanel.CreateBoolPreference("Show Notifications", Color.white, Preferences.ShowNotifications, Preferences.OwnCategory);
        settingsPanel.CreateEnumPreference("Replay Control Mode", Color.white, Preferences.ReplayControlMode, Preferences.OwnCategory);
        settingsPanel.CreateEnumPreference("Replay Control Hand", Color.white, Preferences.ReplayControlHand, Preferences.OwnCategory);
        settingsPanel.CreateFloatPreference("Double Tap Time", Color.white, 0.1f, 0.1f, 1f, Preferences.DoubleTapTime, Preferences.OwnCategory);
        
        #endregion
        
        ConnectHooks();
    }

    private static void ConnectHooks()
    {
        ObsBridge.OnRecordStateChanged += RecordStatusChanged;
        ObsBridge.OnStreamStateChanged += StreamStatusChanged;
        ObsBridge.OnReplayBufferStateChanged += ReplayStatusChanged;
        ObsBridge.OnSceneCreated += SceneCreated;
        ObsBridge.OnSceneRemoved += SceneDeleted;
        ObsBridge.OnReplayBufferSaved += ReplaySaved;
    }

    private static void RecordStatusChanged(object sender, RecordStateChangedEventArgs e)
    {
        SetRecordButton(e.OutputState.IsActive);
        NotificationHandler.SendNotif(e.OutputState.IsActive
            ? NotificationHandler.RecordingStarted
            : NotificationHandler.RecordingStopped);
    }
    
    private static void StreamStatusChanged(object sender, StreamStateChangedEventArgs e)
    {
        SetStreamButton(e.OutputState.IsActive);
        NotificationHandler.SendNotif(e.OutputState.IsActive
            ? NotificationHandler.StreamStarted
            : NotificationHandler.StreamStopped);
    }
    
    private static void ReplayStatusChanged(object sender, ReplayBufferStateChangedEventArgs e)
    {
        SetReplayButton(e.OutputState.IsActive);
        NotificationHandler.SendNotif(e.OutputState.IsActive
            ? NotificationHandler.ReplayBufferStarted
            : NotificationHandler.ReplayBufferStopped);
    }

    private static void SceneCreated(object sender, SceneCreatedEventArgs e)
    {
        var func = _scenesPanel.CreateFunctionElement(e.SceneName, Color.white, () =>
        {
            ObsBridge.SetScene(e.SceneName);
        });
        SceneButtons.Add(e.SceneName, func);
    }
    
    private static void SceneDeleted(object sender, SceneRemovedEventArgs e)
    {
        _scenesPanel.RemoveElement(SceneButtons[e.SceneName]);
        SceneButtons.Remove(e.SceneName);
    }

    private static void ReplaySaved(object sender, ReplayBufferSavedEventArgs e)
    {
        NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
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