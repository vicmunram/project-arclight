using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().mute = PlayerPrefs.GetInt("music") == 1 ? false : true;
    }
}
