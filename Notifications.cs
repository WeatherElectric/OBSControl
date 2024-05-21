using BoneLib.Notifications;

namespace WeatherElectric.OBSControl;

internal static class Notifications
{
    public static void SendNotif(Notification notification)
    {
        if (Preferences.ShowNotifications.Value)
        {
            Notifier.Send(notification);
        }
    }
    public static Notification RecordingStarted { get; } = new()
    {
        Title = "Recording Started",
        Message = "Recording has started.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification RecordingStopped { get; } = new()
    {
        Title = "Recording Stopped",
        Message = "Recording has stopped.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification RecordingPaused { get; } = new()
    {
        Title = "Recording Paused",
        Message = "Recording has been paused.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification RecordingResumed { get; } = new()
    {
        Title = "Recording Resumed",
        Message = "Recording has been resumed.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification StreamStarted { get; } = new()
    {
        Title = "Stream Started",
        Message = "Stream has started.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification StreamStopped { get; } = new()
    {
        Title = "Stream Stopped",
        Message = "Stream has stopped.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification ReplayBufferStarted { get; } = new()
    {
        Title = "Replay Buffer Started",
        Message = "Replay buffer has started.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification ReplayBufferStopped { get; } = new()
    {
        Title = "Replay Buffer Stopped",
        Message = "Replay buffer has stopped.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
    
    public static Notification ReplaySaved { get; } = new()
    {
        Title = "Replay Saved",
        Message = "Replay has been saved.",
        Type = NotificationType.Information,
        PopupLength = 1f,
        ShowTitleOnPopup = true
    };
}