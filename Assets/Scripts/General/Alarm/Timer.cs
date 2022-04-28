using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Timer
{
    public static float globalTime;
    public static float stretchTime;
    public static bool enabled = false;

    public static float GetGlobalTime()
    {
        return globalTime;
    }

    public static void Start(float time)
    {
        globalTime = stretchTime = time;
        enabled = true;
    }

    public static void Restart(float time)
    {
        stretchTime = time;
        globalTime = stretchTime + PlayerPrefs.GetFloat("globalTime");
        enabled = true;
    }

    public static void Reset()
    {
        globalTime = stretchTime + PlayerPrefs.GetFloat("globalTime");
    }

    public static float Add(float time)
    {
        globalTime += time;
        return globalTime;
    }

    public static float Subtract(float time)
    {
        globalTime -= time;
        return globalTime;
    }

    public static string GetFormattedTime()
    {
        float time = GetGlobalTime();
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        return string.Format("{0}:{1}", minutes, seconds);
    }
}
