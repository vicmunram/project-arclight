using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Thanks : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Main Menu");
    }
}
