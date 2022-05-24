using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    void Start()
    {
        InitGraphicSettings();
        InitSoundSettings();
        GameObject.Find("Return").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Return);
    }

    private void ChangeResolution(string value)
    {
        string[] resolution = value.Split('×');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), true);

        PlayerPrefs.SetString("resolution", value);
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

    private void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void InitGraphicSettings()
    {
        Dropdown resolutionDropdown = GameObject.Find("Resolution Dropdown").GetComponent<Dropdown>();
        resolutionDropdown.onValueChanged.AddListener(delegate { ChangeResolution(resolutionDropdown.options[resolutionDropdown.value].text); });
        string[] resolution = PlayerPrefs.GetString("resolution", "1920×1080").Split('×');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), true);

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
        musicToggle.onValueChanged.AddListener(delegate { SaveToggle(musicToggle, "music"); });
        musicToggle.isOn = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
    }

}
