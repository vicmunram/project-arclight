using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class Localization
{
    private static string path = "Translations/";
    public static string textLanguage;
    public static Dictionary<string, string> textTranslations;

    public static void LoadLocalization(bool force = false)
    {
        if (textTranslations == null || force)
        {
            textTranslations = new Dictionary<string, string>();

            textLanguage = PlayerPrefs.GetString("textLanguage", "es");
            string[] txtLines = Resources.Load<TextAsset>(path + "translations_" + textLanguage).text.Split(';');

            foreach (string line in txtLines)
            {
                if (!line.Trim().StartsWith("/"))
                {
                    string[] txtTranslation = line.Split('=');
                    textTranslations.Add(txtTranslation[0].Trim(), txtTranslation[1].Trim());
                }
            }
        }
    }

    public static string GetLocalizedString(string key)
    {
        LoadLocalization();
        return textTranslations[key];
    }

    public static TextAsset GetLocalizedText(string path, string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path + PlayerPrefs.GetString("textLanguage", "es") + "/" + fileName);
        textAsset.text.Replace("<br>", "\n");
        return Resources.Load<TextAsset>(path + PlayerPrefs.GetString("textLanguage", "es") + "/" + fileName);
    }

    public static string GetFormattedText(string text)
    {
        string formattedText = text.Trim().Replace("<br>", "\n");
        return formattedText;
    }

    public static void TranslateTexts(Text[] texts)
    {
        LoadLocalization();

        foreach (Text text in texts)
        {
            if (textTranslations.ContainsKey(text.name))
            {
                text.text = GetLocalizedString(text.name);
            }
        }
    }
}
