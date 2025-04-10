using OBSWebsocketDotNet.Types.Events;
using UnityEngine.Diagnostics;
using WeatherElectric.OBSControl.Handlers;

namespace WeatherElectric.OBSControl.Menu;

internal static class BoneMenu
{
    private static Page _subCat;
    private static FunctionElement _recordButton;
    private static FunctionElement _pauseButton;
    private static FunctionElement _streamButton;
    private static FunctionElement _replayButton;
    private static FunctionElement _connectButton;
    
    private static Page _scenesPanel;
    private static readonly List<FunctionElement> SceneButtons = [];
    
    
    public static void SetupBaseMenu()
    {
        var mainCat = Page.Root.CreatePage("<color=#6FBDFF>Weather Electric</color>", Color.cyan);
        _subCat = mainCat.CreatePage("<color=#284cb8>OBSControl</color>", Color.white);
        _connectButton = _subCat.CreateFunction("If you see this, it failed to connect.", Color.red, () =>
        {
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
        });
    }

    public static void SetupObsControls()
    {
        _subCat.Remove(_connectButton);
        
        #region Recording
        
        var recordPanel = _subCat.CreatePage("Record", Color.green);
        _recordButton = recordPanel.CreateFunction("Record Button", Color.green, () =>
        {
            switch (ObsBridge.IsRecording())
            {
                case true:
                    ObsBridge.StopRecording();
                    NotificationHandler.SendNotif(NotificationHandler.RecordingStopped);
                    break;
                case false:
                    ObsBridge.StartRecording();
                    NotificationHandler.SendNotif(NotificationHandler.RecordingStarted);
                    break;
            }
        });
        SetRecordButton(ObsBridge.IsRecording());
        _pauseButton = recordPanel.CreateFunction("Pause Button", Color.yellow, () =>
        {
            switch (ObsBridge.IsRecordingPaused())
            {
                case true:
                    ObsBridge.ResumeRecording();
                    NotificationHandler.SendNotif(NotificationHandler.RecordingResumed);
                    break;
                case false:
                    ObsBridge.PauseRecording();
                    NotificationHandler.SendNotif(NotificationHandler.RecordingPaused);
                    break;
            }
        });
        SetPauseButton(ObsBridge.IsRecordingPaused());
        
        #endregion
        
        #region Streaming
        
        var streamPanel = _subCat.CreatePage("Stream", Color.blue);
        _streamButton = streamPanel.CreateFunction("Stream Button", Color.blue, () =>
        {
            switch (ObsBridge.IsStreaming())
            {
                case true:
                    ObsBridge.StopStreaming();
                    NotificationHandler.SendNotif(NotificationHandler.StreamStopped);
                    break;
                case false:
                    ObsBridge.StartStreaming();
                    NotificationHandler.SendNotif(NotificationHandler.StreamStarted);
                    break;
            }
        });
        SetStreamButton(ObsBridge.IsStreaming());
        
        #endregion
        
        #region Replay
        
        var replayPanel = _subCat.CreatePage("Replay", Color.yellow);
        _replayButton = replayPanel.CreateFunction("Replay Button", Color.blue, () =>
        {
            switch (ObsBridge.IsReplayBufferActive())
            {
                case true:
                    ObsBridge.StopReplayBuffer();
                    NotificationHandler.SendNotif(NotificationHandler.ReplayBufferStopped);
                    break;
                case false:
                    ObsBridge.StartReplayBuffer();
                    NotificationHandler.SendNotif(NotificationHandler.ReplayBufferStarted);
                    break;
            }
        });
        SetReplayButton(ObsBridge.IsReplayBufferActive());
        replayPanel.CreateFunction("Save Replay", Color.blue, () =>
        {
            ObsBridge.SaveReplayBuffer();
            NotificationHandler.SendNotif(NotificationHandler.ReplaySaved);
        });
        
        #endregion
        
        #region Scenes
        
        _scenesPanel = _subCat.CreatePage("Scenes", Color.red);
        var scenes = ObsBridge.GetScenes();
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        ModConsole.Msg("Finding scenes", 1);
        foreach (var scene in scenes)
        {
            ModConsole.Msg($"Found scene: {scene.Name}", 1);
            var func= _scenesPanel.CreateFunction(scene.Name, Color.white, () =>
            {
                ObsBridge.SetScene(scene.Name);
                NotificationHandler.SceneChanged.Message = $"Scene changed to {scene.Name}";
                NotificationHandler.SendNotif(NotificationHandler.SceneChanged);
            });
            SceneButtons.Add(func);
        }
        
        #endregion
        
        #region Settings
        
        var settingsPanel = _subCat.CreatePage("Settings", Color.gray);
        settingsPanel.CreateBoolPreference("Show Notifications", Color.white, Preferences.ShowNotifications, Preferences.OwnCategory);
        settingsPanel.CreateEnum("Replay Control Hand", Color.white, Preferences.ReplayControlHand.Value, v =>
        {
            Preferences.ReplayControlHand.Value = (ControlHand)v;
            Preferences.OwnCategory.SaveToFile(false);
        });
        settingsPanel.CreateFloatPreference("Double Tap Time", Color.white, 0.1f, 0.1f, 1f, Preferences.DoubleTapTime, Preferences.OwnCategory);
        
        #endregion
        
        ConnectHooks();
        SetButtonStates();
    }

    private static void ConnectHooks()
    {
        ObsBridge.OnRecordStateChanged += RecordStatusChanged;
        ObsBridge.OnStreamStateChanged += StreamStatusChanged;
        ObsBridge.OnReplayBufferStateChanged += ReplayStatusChanged;
        ObsBridge.OnSceneCreated += SceneCreated;
        ObsBridge.OnSceneRemoved += SceneDeleted;
    }

    private static void SetButtonStates()
    {
        var recordState = ObsBridge.IsRecording();
        var streamState = ObsBridge.IsStreaming();
        var replayState = ObsBridge.IsReplayBufferActive();
        
        SetRecordButton(recordState);
        SetStreamButton(streamState);
        SetReplayButton(replayState);
    }

    private static void RecordStatusChanged(object sender, RecordStateChangedEventArgs e)
    {
        SetRecordButton(e.OutputState.IsActive);
    }
    
    private static void StreamStatusChanged(object sender, StreamStateChangedEventArgs e)
    {
        SetStreamButton(e.OutputState.IsActive);
    }
    
    private static void ReplayStatusChanged(object sender, ReplayBufferStateChangedEventArgs e)
    {
        SetReplayButton(e.OutputState.IsActive);
    }

    private static void SceneCreated(object sender, SceneCreatedEventArgs e)
    {
        ModConsole.Msg($"Scene created: {e.SceneName}", 1);
        var func = _scenesPanel.CreateFunction(e.SceneName, Color.white, () =>
        {
            ObsBridge.SetScene(e.SceneName);
        });
        SceneButtons.Add(func);
    }
    
    private static void SceneDeleted(object sender, SceneRemovedEventArgs e)
    {
        ModConsole.Msg($"Scene deleted: {e.SceneName}", 1);
        foreach (var button in SceneButtons.Where(button => button.ElementName == e.SceneName))
        {
            _scenesPanel.Remove(button);
            SceneButtons.Remove(button);
            break;
        }
    }
    
    private static void SetRecordButton(bool isRecording)
    {
        if (isRecording)
        {
            _recordButton.ElementColor = Color.red;
            _recordButton.ElementName = "Stop Recording";
        }
        else
        {
            _recordButton.ElementColor = Color.green;
            _recordButton.ElementName = "Start Recording";
        }
    }
    
    private static void SetPauseButton(bool isPaused)
    {
        if (isPaused)
        {
            _pauseButton.ElementColor = Color.white;
            _pauseButton.ElementName = "Resume Recording";
        }
        else
        {
            _pauseButton.ElementColor = Color.yellow;
            _pauseButton.ElementName = "Pause Recording";
        }
    }

    private static void SetStreamButton(bool isStreaming)
    {
        if (isStreaming)
        {
            _streamButton.ElementColor = Color.red;
            _streamButton.ElementName = "Stop Streaming";
        }
        else
        {
            _streamButton.ElementColor = Color.green;
            _streamButton.ElementName = "Start Streaming";
        }
    }
    
    private static void SetReplayButton(bool isReplayActive)
    {
        if (isReplayActive)
        {
            _replayButton.ElementColor = Color.red;
            _replayButton.ElementName = "Stop Replay Buffer";
        }
        else
        {
            _replayButton.ElementColor = Color.green;
            _replayButton.ElementName = "Start Replay Buffer";
        }
    }
}