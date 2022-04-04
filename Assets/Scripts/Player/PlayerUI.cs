using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public Text formattedTime;

    void Start()
    {
        formattedTime.text = Timer.enabled ? GetFormattedTime() : null;
    }

    void Update()
    {
        string time = GetFormattedTime();
        if (Timer.enabled && !time.Equals(formattedTime.text))
        {
            formattedTime.text = time;
        }
    }

    private string GetFormattedTime()
    {
        float time = Timer.GetGlobalTime();
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        return string.Format("{0}:{1}", minutes, seconds);
    }

}
