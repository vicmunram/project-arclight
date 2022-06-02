using UnityEngine;
using UnityEngine.SceneManagement;

public static class AudioUtils
{
    public static string effectsPath = "Effects/";
    public static string musicPath = "Music/";
    public static void PlayEffect(GameObject gameobject, bool atPoint)
    {
        AudioSource audioSource = gameobject.GetComponent<AudioSource>();
        if (audioSource != null && PlayerPrefs.GetInt("sound") == 1)
        {
            if (audioSource.loop)
            {
                audioSource.Play();
            }
            else if (!atPoint)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, Camera.main.transform.position, audioSource.volume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, gameobject.transform.position, audioSource.volume);
            }
        }
    }

    public static void PlayEffect(GameObject gameobject, bool atPoint, string clipName, bool loop)
    {
        AudioSource audioSource = gameobject.GetComponent<AudioSource>();
        audioSource.loop = loop;
        SetEffect(audioSource, clipName);
        PlayEffect(gameobject, atPoint);
    }

    public static AudioClip GetEffect(string clipName)
    {
        return Resources.Load<AudioClip>(effectsPath + clipName);
    }
    public static void SetEffect(GameObject gameObject, string clipName)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = GetEffect(clipName);
    }

    public static void SetEffect(AudioSource audioSource, string clipName)
    {
        audioSource.clip = GetEffect(clipName);
    }

    public static void PlayEffect(string clipName, float volume = 0.15f, bool atPoint = false)
    {
        AudioSource.PlayClipAtPoint(GetEffect(clipName), Camera.main.transform.position, volume);
    }

    public static void ToggleMusic()
    {
        AudioSource audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.mute = PlayerPrefs.GetInt("music", 1) == 1 ? false : true;
        }
    }

    public static void PlayMusic(AudioClip clip)
    {
        AudioSource audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public static void PlayMusic(string clipName)
    {
        AudioSource audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(musicPath + clipName);
        audioSource.Play();
    }

    public static void PlaySectionMusic()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("0"))
        {
            AudioUtils.PlayMusic("Section0");
        }
        else if (sceneName.StartsWith("1"))
        {
            AudioUtils.PlayMusic("Section1");
        }
        else if (sceneName.StartsWith("2"))
        {
            AudioUtils.PlayMusic("Section2");
        }
        else if (sceneName.StartsWith("3"))
        {
            AudioUtils.PlayMusic("Section3");
        }
    }

    public static void StopMusic()
    {
        AudioSource audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        audioSource.Stop();
    }
}
