using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;

namespace WeatherElectric.OBSControl.OBS;

/// <summary>
/// Allows for control over OBS through it's websocket server.
/// </summary>
public static class ObsBridge
{
    #region Private
    
    private static readonly OBSWebsocket Obs = new();
    
    #region Hook Triggers
    
    private static void RecordStateChanged(object sender, RecordStateChangedEventArgs e)
    {
        OnRecordStateChanged?.Invoke(sender, e);
    }
    
    private static void ReplaySaved(object sender, ReplayBufferSavedEventArgs e)
    {
        OnReplayBufferSaved?.Invoke(sender, e);
    }

    private static void ReplayStateChanged(object sender, ReplayBufferStateChangedEventArgs e)
    {
        OnReplayBufferStateChanged?.Invoke(sender, e);
    }
    
    private static void StreamStateChanged(object sender, StreamStateChangedEventArgs e)
    {
        OnStreamStateChanged?.Invoke(sender, e);
    }

    private static void SceneChanged(object sender, ProgramSceneChangedEventArgs e)
    {
        OnSceneChanged?.Invoke(sender, e);
    }

    private static void VirtualCamStateChanged(object sender, VirtualcamStateChangedEventArgs e)
    {
        OnVirtualCamStateChanged?.Invoke(sender, e);
    }
    
    private static void SceneCreated(object sender, SceneCreatedEventArgs e)
    {
        OnSceneCreated?.Invoke(sender, e);
    }
    
    private static void SceneRemoved(object sender, SceneRemovedEventArgs e)
    {
        // Not implemented
    }

    #endregion
    
    #endregion

    #region Internal
    
    internal static void Connect()
    {
        ModConsole.Msg("Attempting to connect to OBS...", 1);
        try
        {
            Obs.ConnectAsync(Preferences.WebsocketURL.Value, Preferences.WebsocketPassword.Value);
        }
        catch (AuthFailureException)
        {
            ModConsole.Error("Failed to authenticate with OBS. Check your password.");
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to connect to OBS. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to connect to OBS. Error: {e.Message}");
        }
        ModConsole.Msg("If you still aren't connected, it's likely the wrong password.", 1);
    }

    internal static void Disconnect()
    {
        ModConsole.Msg("Disconnecting from OBS...", 1);
        Obs.Disconnect();
    }

    internal static void InitHooks()
    {
        Obs.RecordStateChanged += RecordStateChanged;
        Obs.ReplayBufferSaved += ReplaySaved;
        Obs.ReplayBufferStateChanged += ReplayStateChanged;
        Obs.StreamStateChanged += StreamStateChanged;
        Obs.CurrentProgramSceneChanged += SceneChanged;
        Obs.VirtualcamStateChanged += VirtualCamStateChanged;
        Obs.SceneCreated += SceneCreated;
        Obs.SceneRemoved += SceneRemoved;
        Obs.Connected += ObsConnected;
    }

    private static void ObsConnected(object sender, EventArgs e)
    {
        ModConsole.Msg("OBS connected!", 1);
        BoneMenu.SetupObsControls();
    }
    
    #endregion
    
    #region Public
    
    #region Hooks

    /// <summary>
    /// Called when the recording state of OBS changes.
    /// </summary>
    public static Action<object, RecordStateChangedEventArgs> OnRecordStateChanged;
    /// <summary>
    /// Called when the replay buffer is saved in OBS.
    /// </summary>
    public static Action<object, ReplayBufferSavedEventArgs> OnReplayBufferSaved;
    /// <summary>
    /// Called when the replay buffer state of OBS changes.
    /// </summary>
    public static Action<object, ReplayBufferStateChangedEventArgs> OnReplayBufferStateChanged;
    /// <summary>
    /// Called when the streaming state of OBS changes.
    /// </summary>
    public static Action<object, StreamStateChangedEventArgs> OnStreamStateChanged;
    /// <summary>
    /// Called when the current scene in OBS changes.
    /// </summary>
    public static Action<object, ProgramSceneChangedEventArgs> OnSceneChanged;
    /// <summary>
    /// Called when a new scene is created in OBS.
    /// </summary>
    public static Action<object, SceneCreatedEventArgs> OnSceneCreated;
    /// <summary>
    /// Called when the virtual cam state of OBS changes.
    /// </summary>
    public static Action<object, VirtualcamStateChangedEventArgs> OnVirtualCamStateChanged;
    /// <summary>
    /// Called when a scene is removed in OBS.
    /// </summary>
    public static Action<object, SceneRemovedEventArgs> OnSceneRemoved;
    
    #endregion
    
    #region Scenes

    /// <summary>
    /// Get a list of all scenes in the current OBS profile.
    /// </summary>
    /// <returns>List of SceneBasicInfo</returns>
    public static List<SceneBasicInfo> GetScenes()
    {
        var scenes = Obs.GetSceneList();
        return scenes.Scenes;
    }
    
    /// <summary>
    /// Sets the active scene in OBS to the specified scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to</param>
    public static void SetScene(string sceneName)
    {
        Obs.SetCurrentProgramScene(sceneName);
    }
    
    #endregion
    
    #region Statuses
    
    /// <summary>
    /// Check if OBS websocket is connected.
    /// </summary>
    /// <returns>True if connected, false if disconnected</returns>
    public static bool IsConnected()
    {
        return Obs.IsConnected;
    }

    /// <summary>
    /// Get the recording status of OBS.
    /// </summary>
    /// <returns>True if recording, false if not recording</returns>
    public static bool IsRecording()
    {
        var status = Obs.GetRecordStatus();
        return status.IsRecording;
    }

    /// <summary>
    /// Gets the recording paused status of OBS.
    /// </summary>
    /// <returns>True if recording is paused, false if not paused</returns>
    public static bool IsRecordingPaused()
    {
        var status = Obs.GetRecordStatus();
        return status.IsRecordingPaused;
    }

    /// <summary>
    /// Get the streaming status of OBS.
    /// </summary>
    /// <returns>True if stremaing, false if not streaming</returns>
    public static bool IsStreaming()
    {
        var status = Obs.GetStreamStatus();
        return status.IsActive;
    }

    /// <summary>
    /// Get the replay buffer status of OBS.
    /// </summary>
    /// <returns>True if active, false if inactive</returns>
    public static bool IsReplayBufferActive()
    {
        return Obs.GetReplayBufferStatus();
    }

    /// <summary>
    /// Get the virtual cam status of OBS.
    /// </summary>
    /// <returns>True if active, false if inactive</returns>
    public static bool IsVirtualCamActive()
    {
        var status = Obs.GetVirtualCamStatus();
        return status.IsActive;
    }
    
    #endregion
    
    #region Recording
    
    /// <summary>
    /// Start recording in OBS.
    /// </summary>
    public static void StartRecording()
    {
        if (IsRecording()) return;
        ModConsole.Msg("Starting recording...", 1);
        try
        {
            Obs.StartRecord();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to start recording. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to start recording. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Pauses recording in OBS.
    /// </summary>
    public static void PauseRecording()
    {
        if (IsRecordingPaused()) return;
        ModConsole.Msg("Pausing recording...", 1);
        try
        {
            Obs.PauseRecord();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to pause recording. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to pause recording. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Resumes recording in OBS.
    /// </summary>
    public static void ResumeRecording()
    {
        if (!IsRecordingPaused()) return;
        ModConsole.Msg("Resuming recording...", 1);
        try
        {
            Obs.ResumeRecord();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to resume recording. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to resume recording. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Stops recording in OBS.
    /// </summary>
    public static void StopRecording()
    {
        if (!IsRecording()) return;
        ModConsole.Msg("Stopping recording...", 1);
        try
        {
            Obs.StopRecord();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to stop recording. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to stop recording. Error: {e.Message}");
        }
    }
    
    #endregion
    
    #region Streaming
    
    /// <summary>
    /// Starts streaming in OBS.
    /// </summary>
    public static void StartStreaming()
    {
        if (IsStreaming()) return;
        ModConsole.Msg("Starting stream...", 1);
        try
        {
            Obs.StartStream();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to start streaming. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to start streaming. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Stops streaming in OBS.
    /// </summary>
    public static void StopStreaming()
    {
        if (!IsStreaming()) return;
        ModConsole.Msg("Stopping stream...", 1);
        try
        {
            Obs.StopStream();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to stop streaming. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to stop streaming. Error: {e.Message}");
        }
    }
    
    #endregion
    
    #region Replay Buffer
    
    /// <summary>
    /// Starts the replay buffer in OBS.
    /// </summary>
    public static void StartReplayBuffer()
    {
        if (IsReplayBufferActive()) return;
        ModConsole.Msg("Starting replay buffer...", 1);
        try
        {
            Obs.StartReplayBuffer();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to start replay buffer. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to start replay buffer. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Stops the replay buffer in OBS.
    /// </summary>
    public static void StopReplayBuffer()
    {
        if (!IsReplayBufferActive()) return;
        ModConsole.Msg("Stopping replay buffer...", 1);
        try
        {
            Obs.StopReplayBuffer();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to stop replay buffer. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to stop replay buffer. Error: {e.Message}");
        }
    }
    
    /// <summary>
    /// Saves the replay buffer in OBS.
    /// </summary>
    public static void SaveReplayBuffer()
    {
        if (!IsReplayBufferActive()) return;
        ModConsole.Msg("Saving replay buffer...", 1);
        try
        {
            Obs.SaveReplayBuffer();
        }
        catch (ErrorResponseException e)
        {
            ModConsole.Error($"Failed to save replay buffer. Error: {e.Message}");
        }
        catch (Exception e)
        {
            ModConsole.Error($"Failed to save replay buffer. Error: {e.Message}");
        }
    }
    
    #endregion
    
    #endregion
}