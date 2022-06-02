using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private bool first = true;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().mute = PlayerPrefs.GetInt("music") == 1 ? false : true;
    }

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("CP"))
        {
            if (first)
            {
                if (sceneName.StartsWith("0"))
                {
                    AudioUtils.PlayMusic("Section0");
                }
                else if (sceneName.StartsWith("1") && !sceneName.Equals("1-10-CP"))
                {
                    AudioUtils.PlayMusic("Section1");
                }
                else if (sceneName.StartsWith("2") && !sceneName.Equals("2-9-CP"))
                {
                    AudioUtils.PlayMusic("Section2");
                }
                else if (sceneName.StartsWith("3"))
                {
                    AudioUtils.PlayMusic("Section3");

                }
                first = false;
            }
        }
        else
        {
            first = true;
        }
    }
}
