namespace WeatherElectric.OBSControl.Menu;

internal static class BoneMenuExtensions
{
    #region Bool

    public static BoolElement CreateBoolPreference(this Page category, string name, Color color,
        MelonPreferences_Entry<bool> pref, MelonPreferences_Category prefCategory, bool autoSave = true)
    {
        var boolElement = category.CreateBool(name, color, pref.Value, v =>
        {
            pref.Value = v;
            if (autoSave) prefCategory.SaveToFile(false);
        });
        return boolElement;
    }

    #endregion

    #region Float

    public static FloatElement CreateFloatPreference(this Page category, string name, Color color,
        float increment, float min, float max, MelonPreferences_Entry<float> pref,
        MelonPreferences_Category prefCategory, bool autoSave = true)
    {
        var floatElement = category.CreateFloat(name, color, pref.Value, increment, min, max, v =>
        {
            pref.Value = v;
            if (autoSave) prefCategory.SaveToFile(false);
        });
        return floatElement;
    }
    
    #endregion

    #region Int

    public static IntElement CreateIntPreference(this Page category, string name, Color color, int increment,
        int min, int max, MelonPreferences_Entry<int> pref, MelonPreferences_Category prefCategory,
        bool autoSave = true)
    {
        var intElement = category.CreateInt(name, color, pref.Value, increment, min, max, v =>
        {
            pref.Value = v;
            if (autoSave) prefCategory.SaveToFile(false);
        });
        return intElement;
    }
    
    #endregion

    #region Enum

    public static EnumElement CreateEnumPreference(this Page category, string name, Color color,
        Enum pref, MelonPreferences_Category prefCategory, bool autoSave = true)
    {
        var enumElement = category.CreateEnum(name, color, pref, v =>
        {
            pref = v;
            if (autoSave) prefCategory.SaveToFile(false);
        });
        return enumElement;
    }

    #endregion
}