using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Thanks : MonoBehaviour
{
    void Awake()
    {
        Localization.TranslateTexts(GameObject.FindObjectsOfType<Text>());
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        AudioUtils.PlayMusic("MainMenu");
        SceneManager.LoadScene("Main Menu");
    }
}
