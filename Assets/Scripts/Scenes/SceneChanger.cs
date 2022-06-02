using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string newLevel;
    public AudioClip clip;
    public bool stopMusic;

    void Start()
    {
        if (newLevel == "next" || newLevel == "new")
        {
            string[] activeScene = SceneManager.GetActiveScene().name.Split('-');
            string section = activeScene[0];
            string level = "1";

            if (newLevel == "next")
            {
                level = (Int32.Parse(activeScene[1]) + 1).ToString();
            }
            else if (newLevel == "new")
            {
                section = (Int32.Parse(activeScene[0]) + 1).ToString();
            }

            newLevel = section + "-" + level;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (clip != null)
            {
                AudioUtils.PlayMusic(clip);
            }
            else if (stopMusic)
            {
                AudioUtils.StopMusic();
            }
            SceneManager.LoadScene(newLevel);
        }
    }
}
