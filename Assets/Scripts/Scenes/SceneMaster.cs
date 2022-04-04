using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static void Respawn()
    {
        string checkpoint = PlayerPrefs.GetString("checkpoint");
        SceneManager.LoadScene(checkpoint);
        Timer.Reset();
    }
}
