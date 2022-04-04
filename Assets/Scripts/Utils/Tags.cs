using System.Collections.Generic;
public static class Tags
{
    // Object tags
    public static string interactable = "Interactable";
    public static string solid = "Solid";
    public static string transparent = "Transparent";
    public static string mirror = "Mirror";
    public static string danger = "Danger";
    public static string abysm = "Abysm";
    public static string breakable = "Breakable";
    public static string breakableOneSide = "BreakableOneSide";
    public static string sceneChanger = "SceneChanger";
    public static string any = "Any";
    public static List<string> alwaysSolid = new List<string> { solid, breakable, breakableOneSide, mirror };

    // Scene tags
    public static string punchEnabler = "CP";
    public static string dashEnabler = "CD";
}
