using UnityEngine;

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
        globalTime = stretchTime + GameProgress.LoadProgress().currentAccumulatedTime;
        enabled = true;
    }

    public static void Reset()
    {
        SaveData saveData = GameProgress.LoadProgress();
        globalTime = saveData.currentStretchTime + saveData.currentAccumulatedTime;
    }

    public static float Add(float time)
    {
        globalTime += time;
        GameObject.Find("Player UI").GetComponent<PlayerUI>().AddTime(time);
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
