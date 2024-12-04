using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/save.dat";

    public static void SavePlayerStats(PlayerStatsSO playerStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(saveFilePath, FileMode.Create);

        PlayerData data = new PlayerData(playerStats);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static void LoadPlayerStats(PlayerStatsSO playerStats)
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(saveFilePath, FileMode.Open);
            
            PlayerData data = (PlayerData)formatter.Deserialize(fileStream);
            fileStream.Close();

            data.ApplyTo(playerStats);
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }
}
