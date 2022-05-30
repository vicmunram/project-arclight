using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    void Start()
    {
        Localization.TranslateTexts(GameObject.FindObjectsOfType<Text>());
        InitGraphicSettings();
        InitSoundSettings();
        InitLanguageSettings();
        GameObject.Find("Return").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Return);
    }

    private void ChangeLanguage(string value)
    {
        PlayerPrefs.SetString("textLanguage", value == "English" ? "en" : "es");
        Localization.LoadLocalization();
        Localization.TranslateTexts(GameObject.FindObjectsOfType<Text>());
    }

    private void ToggleFullscreen(Toggle toggle)
    {
        Screen.fullScreen = toggle.isOn;
        PlayerPrefs.SetInt("fullscreen", toggle.isOn ? 1 : 0);
    }

    private void SaveToggle(Toggle toggle, string name)
    {
        PlayerPrefs.SetInt(name, toggle.isOn ? 1 : 0);
    }
    private void SaveToggleMusic(Toggle toggle, string name)
    {
        SaveToggle(toggle, name);
        AudioUtils.ToggleMusic();
    }

    private void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void InitGraphicSettings()
    {
        Toggle fullscreenToggle = GameObject.Find("Toggle Fullscreen").GetComponent<Toggle>();
        fullscreenToggle.onValueChanged.AddListener(delegate { ToggleFullscreen(fullscreenToggle); });
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1 ? true : false;
    }

    private void InitSoundSettings()
    {
        Toggle soundToggle = GameObject.Find("Toggle Sound").GetComponent<Toggle>();
        soundToggle.onValueChanged.AddListener(delegate { SaveToggle(soundToggle, "sound"); });
        soundToggle.isOn = PlayerPrefs.GetInt("sound", 1) == 1 ? true : false;

        Toggle musicToggle = GameObject.Find("Toggle Music").GetComponent<Toggle>();
        musicToggle.onValueChanged.AddListener(delegate { SaveToggleMusic(musicToggle, "music"); });
        musicToggle.isOn = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;

        AudioUtils.ToggleMusic();
    }

    private void InitLanguageSettings()
    {
        Dropdown textDropdown = GameObject.Find("Text Dropdown").GetComponent<Dropdown>();
        textDropdown.onValueChanged.AddListener(delegate { ChangeLanguage(textDropdown.options[textDropdown.value].text); });
        textDropdown.value = PlayerPrefs.GetString("textLanguage", "es") == "es" ? 0 : 1;
    }

    private void SetTextLanguage()
    {
        string language = PlayerPrefs.GetString("textLanguage", "es");
    }
}
