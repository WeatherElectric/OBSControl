using System.IO;

namespace WeatherElectric.OBSControl.Melon;

internal static class UserData
{
    private static string ObsWebSocketPath => Path.Combine(MelonUtils.UserDataDirectory, "obs-websocket.dll");
    private static string WatsonWebsocketPath => Path.Combine(MelonUtils.UserDataDirectory, "WatsonWebsocket.dll");
    
    public static void Setup()
    {
        if (!File.Exists(ObsWebSocketPath))
        {
            var bytes = HelperMethods.GetResourceBytes(Main.ModAsm, "WeatherElectric.OBSControl.Resources.obs-websocket.dll");
            File.WriteAllBytes(ObsWebSocketPath, bytes);
        }
        
        if (!File.Exists(WatsonWebsocketPath))
        {
            var bytes = HelperMethods.GetResourceBytes(Main.ModAsm, "WeatherElectric.OBSControl.Resources.WatsonWebsocket.dll");
            File.WriteAllBytes(WatsonWebsocketPath, bytes);
        }
    }
}