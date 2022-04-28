using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GameProgress
{
    public static string SAVES_FOLDER = "Saves";
    public static string SAVE_NAME = "save";

    public static void SaveProgress(float accumulatedTime, float stretchTime, bool exclude)
    {
        if (!Directory.Exists(SAVES_FOLDER))
        {
            Directory.CreateDirectory(SAVES_FOLDER);
        }

        FileStream save;
        SaveData saveData;
        BinaryFormatter formatter = new BinaryFormatter();

        if (!File.Exists(SAVES_FOLDER + "/" + SAVE_NAME + ".dat"))
        {
            save = File.Create(SAVES_FOLDER + "/" + SAVE_NAME + ".dat");
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

            save = File.Create(SAVES_FOLDER + "/" + SAVE_NAME + ".dat");
        }

        formatter.Serialize(save, saveData);

        save.Close();
    }

    public static SaveData LoadProgress()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream save = File.Open(SAVES_FOLDER + "/" + SAVE_NAME + ".dat", FileMode.Open);
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

        FileStream save = File.Create(SAVES_FOLDER + "/" + SAVE_NAME + ".dat");
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(save, saveData);
        save.Close();
    }

}
