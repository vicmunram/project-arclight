using UnityEngine;

public static class AudioUtils
{
    public static string effectsPath = "Effects/";
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

    public static void PlayEffect(string clipName, float volume)
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(effectsPath + clipName), Camera.main.transform.position, volume);
    }

    public static void SetEffect(AudioSource audioSource, string clipName)
    {
        audioSource.clip = Resources.Load<AudioClip>(effectsPath + clipName);
    }

    public static void ToggleMusic()
    {
        AudioSource audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.mute = PlayerPrefs.GetInt("music", 1) == 1 ? false : true;
        }
    }
}
