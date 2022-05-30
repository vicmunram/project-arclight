using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
