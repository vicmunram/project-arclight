﻿using UnityEngine;
using System.IO;
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
            GameObject.Find("Continue").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            GameObject.Find("Continue").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Continue);
        }

        GameObject.Find("New Game").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NewGame);
        GameObject.Find("Options").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Options);
        GameObject.Find("Quit").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(QuitGame);
    }

    private void NewGame()
    {
        if (!File.Exists(GameProgress.GetFullPath()))
        {
            SceneManager.LoadScene("0-1");
        }
        else
        {
            disclaimerPanel.SetActive(true);
            GameObject.Find("Yes").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Yes);
            GameObject.Find("No").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(No);
        }
    }

    private void Yes()
    {
        GameProgress.DeleteProgress();
        SceneManager.LoadScene("0-1");
    }

    private void No()
    {
        disclaimerPanel.SetActive(false);
    }

    private void Continue()
    {
        GameProgress.LoadLastCheckpoint();
    }

    private void Options()
    {
        SceneManager.LoadScene("Options");
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void LoadConfiguration()
    {
        string[] resolution = PlayerPrefs.GetString("resolution", "1920×1080").Split('×');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), true);
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen", 1) == 1 ? true : false;
    }
}
