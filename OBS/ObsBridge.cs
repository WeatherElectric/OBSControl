using OBSWebsocketDotNet;

namespace WeatherElectric.OBSControl.OBS;

internal static class ObsBridge
{
    private static readonly OBSWebsocket Obs = new();

    public static void Connect()
    {
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
    }
    
    public static void Disconnect()
    {
        Obs.Disconnect();
    }

    public static bool IsRecording()
    {
        var status = Obs.GetRecordStatus();
        return status.IsRecording;
    }

    public static bool IsRecordingPaused()
    {
        var status = Obs.GetRecordStatus();
        return status.IsRecordingPaused;
    }

    public static bool IsStreaming()
    {
        var status = Obs.GetStreamStatus();
        return status.IsActive;
    }

    public static bool IsReplayBufferActive()
    {
        return Obs.GetReplayBufferStatus();
    }

    public static void StartRecording()
    {
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
    
    public static void PauseRecording()
    {
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
    
    public static void ResumeRecording()
    {
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
    
    public static void StopRecording()
    {
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
    
    public static void StartStreaming()
    {
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
    
    public static void StopStreaming()
    {
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
    
    public static void StartReplayBuffer()
    {
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
    
    public static void StopReplayBuffer()
    {
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
    
    public static void SaveReplayBuffer()
    {
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
}