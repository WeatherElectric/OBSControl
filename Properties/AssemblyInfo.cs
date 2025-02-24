[assembly: AssemblyTitle(WeatherElectric.OBSControl.Main.Description)]
[assembly: AssemblyDescription(WeatherElectric.OBSControl.Main.Description)]
[assembly: AssemblyCompany(WeatherElectric.OBSControl.Main.Company)]
[assembly: AssemblyProduct(WeatherElectric.OBSControl.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + WeatherElectric.OBSControl.Main.Author)]
[assembly: AssemblyTrademark(WeatherElectric.OBSControl.Main.Company)]
[assembly: AssemblyVersion(WeatherElectric.OBSControl.Main.Version)]
[assembly: AssemblyFileVersion(WeatherElectric.OBSControl.Main.Version)]
[assembly:
    MelonInfo(typeof(WeatherElectric.OBSControl.Main), WeatherElectric.OBSControl.Main.Name,
        WeatherElectric.OBSControl.Main.Version,
        WeatherElectric.OBSControl.Main.Author, WeatherElectric.OBSControl.Main.DownloadLink)]
[assembly: MelonColor(255, 0, 0, 255)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]