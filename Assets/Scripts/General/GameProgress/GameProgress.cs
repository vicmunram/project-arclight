using System.IO;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GameProgress
{
    public static string SAVES_FOLDER = "Saves";
    public static string SAVE_NAME = "save.dat";

    public static void SaveProgress(float accumulatedTime, float stretchTime, bool exclude)
    {
        string path = Application.persistentDataPath + "/" + SAVES_FOLDER;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream save;
        SaveData saveData;
        BinaryFormatter formatter = new BinaryFormatter();

        path = path + "/" + SAVE_NAME;

        if (!File.Exists(path))
        {
            save = File.Create(path);
            saveData = new SaveData(0, 1, accumulatedTime, stretchTime);
        }
        else
        {
            saveData = LoadProgress();

            string[] sceneData = SceneManager.GetActiveScene().name.Split('-');

            saveData.currentSection = Int32.Parse(sceneData[0]);
            saveData.currentCheckpointLevel = Int32.Parse(sceneData[1]);
            saveData.currentAccumulatedTime = accumulatedTime;
            saveData.currentStretchTime = stretchTime;

            if (!saveData.previousCheckpoints.ContainsKey(saveData.currentSection))
            {
                saveData.previousCheckpoints[saveData.currentSection] = new Dictionary<int, TimeData>();
            }

            saveData.previousCheckpoints[saveData.currentSection][saveData.currentCheckpointLevel] = new TimeData(saveData.currentAccumulatedTime, saveData.currentStretchTime);

            if (exclude)
            {
                if (!saveData.nonTerminalCheckpoints.ContainsKey(saveData.currentSection))
                {
                    saveData.nonTerminalCheckpoints[saveData.currentSection] = new List<int>();
                }

                saveData.nonTerminalCheckpoints[saveData.currentSection].Add(saveData.currentCheckpointLevel);
            }

            save = File.Create(path);
        }

        formatter.Serialize(save, saveData);

        save.Close();
    }

    public static SaveData LoadProgress()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream save = File.Open(GetFullPath(), FileMode.Open);
        SaveData saveData = (SaveData)formatter.Deserialize(save);
        save.Close();

        return saveData;
    }

    public static void LoadCheckpoint(KeyValuePair<int, TimeData> checkpoint)
    {
        SaveData saveData = LoadProgress();
        saveData.currentCheckpointLevel = checkpoint.Key;
        saveData.currentAccumulatedTime = checkpoint.Value.accumulatedTime;
        saveData.currentStretchTime = checkpoint.Value.stretchTime;

        FileStream save = File.Create(GetFullPath());
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(save, saveData);
        save.Close();
    }

    public static void LoadLastCheckpoint()
    {
        SaveData saveData = LoadProgress();
        string checkpoint = saveData.GetCurrentCheckpointSceneName();
        SceneManager.LoadScene(checkpoint);

        if (saveData.currentStretchTime != 0)
        {
            Timer.enabled = true;
            Timer.Reset();
        }
    }

    public static void DeleteProgress()
    {
        File.Delete(GetFullPath());
    }

    public static string GetFullPath()
    {
        return Application.persistentDataPath + "/" + SAVES_FOLDER + "/" + SAVE_NAME;
    }

}
