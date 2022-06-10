using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        InitCanvas();
        Localization.TranslateTexts(GameObject.FindObjectsOfType<Text>());
        InitGraphicSettings();
        InitSoundSettings();
        GameObject.Find("Return").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Return);
        GameObject.Find("Quit").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(QuitGame);
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

    private void SaveToggleMusic(Toggle toggle, string name)
    {
        SaveToggle(toggle, name);
        AudioUtils.ToggleMusic();
    }

    private void Return()
    {
        Time.timeScale = 1;
        AudioUtils.PlayMusic("MainMenu");
        StartCoroutine(PlayEffectAndLoadScene("Main Menu"));
    }

    private void QuitGame()
    {
        AudioUtils.PlayEffect("menuButton");
        Application.Quit();
    }

    private void InitCanvas()
    {
        Canvas canvas = gameObject.GetComponentInParent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
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
    }

    IEnumerator PlayEffectAndLoadScene(string scene)
    {
        AudioUtils.PlayEffect("menuButton");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(scene);
    }

}
