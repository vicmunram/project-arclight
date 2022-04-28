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
        formattedTime.text = Timer.enabled ? Timer.GetFormattedTime() : null;
    }

    void Update()
    {
        string time = Timer.GetFormattedTime();
        if (Timer.enabled && !time.Equals(formattedTime.text))
        {
            formattedTime.text = time;
        }
    }

}
