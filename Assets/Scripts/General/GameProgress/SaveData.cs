using System;
using System.Collections.Generic;

[Serializable]
public struct SaveData
{
    public int currentSection;
    public int currentCheckpointLevel;
    public float currentAccumulatedTime;
    public float currentStretchTime;
    public Dictionary<int, Dictionary<int, TimeData>> previousCheckpoints;
    public Dictionary<int, List<int>> nonTerminalCheckpoints;

    public SaveData(int section, int level, float accumulatedTime, float stretchTime)
    {
        this.currentSection = section;
        this.currentCheckpointLevel = level;
        this.currentAccumulatedTime = accumulatedTime;
        this.currentStretchTime = stretchTime;

        this.previousCheckpoints = new Dictionary<int, Dictionary<int, TimeData>>();
        this.previousCheckpoints[section] = new Dictionary<int, TimeData>();
        this.previousCheckpoints[section][level] = new TimeData(accumulatedTime, stretchTime);

        this.nonTerminalCheckpoints = new Dictionary<int, List<int>>();
    }

    public string GetCheckpointSceneName(int section, int level)
    {
        return section + "-" + level + "-CP";
    }

    public string GetCurrentCheckpointSceneName()
    {
        return GetCheckpointSceneName(currentSection, currentCheckpointLevel);
    }

    public Dictionary<int, TimeData> GetAvailableCheckpoints()
    {
        Dictionary<int, TimeData> availableCheckpoints = new Dictionary<int, TimeData>();

        if (nonTerminalCheckpoints.ContainsKey(currentSection))
        {
            List<int> excluded = nonTerminalCheckpoints[currentSection];

            foreach (KeyValuePair<int, TimeData> checkpoint in previousCheckpoints[currentSection])
            {
                if (!excluded.Contains(checkpoint.Key))
                {
                    availableCheckpoints.Add(checkpoint.Key, checkpoint.Value);
                }
            }
        }
        else
        {
            availableCheckpoints = previousCheckpoints[currentSection];
        }

        return availableCheckpoints;
    }

}

[Serializable]
public struct TimeData
{
    public float accumulatedTime;
    public float stretchTime;

    public TimeData(float accumulatedTime, float stretchTime)
    {
        this.accumulatedTime = accumulatedTime;
        this.stretchTime = stretchTime;
    }
}
