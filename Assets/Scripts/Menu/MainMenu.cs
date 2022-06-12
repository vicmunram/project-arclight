using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static string SAVES_FOLDER = "Saves";
    public static string SAVE_NAME = "save";
    public GameObject disclaimerPanel;

    void Awake()
    {
        LoadConfiguration();
    }
    void Start()
    {
        if (!File.Exists(GameProgress.GetFullPath()))
        {
            GameObject.Find("Continue Bt").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            GameObject.Find("Continue Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Continue);
        }

        GameObject.Find("New Game Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NewGame);
        GameObject.Find("Options Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Options);
        GameObject.Find("Quit Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(QuitGame);
    }

    private void NewGame()
    {

        if (!File.Exists(GameProgress.GetFullPath()))
        {
            StartCoroutine(PlayEffectAndLoadScene("0-1"));
            AudioUtils.StopMusic();
        }
        else
        {
            AudioUtils.PlayEffect("menuButton");
            disclaimerPanel.SetActive(true);
            Localization.TranslateTexts(disclaimerPanel.GetComponentsInChildren<Text>());
            GameObject.Find("Yes Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Yes);
            GameObject.Find("No Bt").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(No);
        }
    }

    private void Yes()
    {
        GameProgress.DeleteProgress();
        StartCoroutine(PlayEffectAndLoadScene("0-1"));
        AudioUtils.StopMusic();
    }

    private void No()
    {
        AudioUtils.PlayEffect("menuButton");
        disclaimerPanel.SetActive(false);
    }

    private void Continue()
    {
        AudioUtils.PlayEffect("menuButton");
        GameProgress.LoadLastCheckpoint();
        AudioUtils.StopMusic();
    }

    private void Options()
    {
        StartCoroutine(PlayEffectAndLoadScene("Options"));
    }

    private void QuitGame()
    {
        AudioUtils.PlayEffect("menuButton");
        Application.Quit();
    }

    private void LoadConfiguration()
    {
        Application.targetFrameRate = 120;
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen", 1) == 1 ? true : false;

        Localization.LoadLocalization();
        Localization.TranslateTexts(GameObject.FindObjectsOfType<Text>());

        AudioUtils.ToggleMusic();
    }

    IEnumerator PlayEffectAndLoadScene(string scene)
    {
        AudioUtils.PlayEffect("menuButton");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(scene);
    }

}