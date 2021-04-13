using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveUtility
{
    private static string saveFilePath = "/player_data.elements";
    
    public static void SavePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string absolutePath = Application.persistentDataPath + saveFilePath;
        FileStream stream = new FileStream(absolutePath, FileMode.Create);

        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadPlayerData()
    {
        string absolutePath = Application.persistentDataPath + saveFilePath;
        if (File.Exists(absolutePath))
        {  
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(absolutePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            data.LoadGame();
        }
        else
        {
            Debug.LogError("Save file not found at " + absolutePath);
        }
    }
}
