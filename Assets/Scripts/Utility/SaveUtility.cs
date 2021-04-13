using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveUtility
{
    public static string saveFilePrefix = "/player_data";
    public static string saveFileSuffix = ".elements";
    
    public static void SavePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string absolutePath = Application.persistentDataPath + GetSaveFilePath(System.DateTime.Now);

        FileStream stream = null;
        try
        {
            stream = new FileStream(absolutePath, FileMode.Create);
            PlayerData data = new PlayerData();

            formatter.Serialize(stream, data);
        }
        catch (System.Exception e)
        {
            Debug.Log("PlayerData: " + e.Message);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }

    /**
     * Load the most recent save file.
     */
    public static void LoadPlayerData()
    {
        string saveDirPath = Application.persistentDataPath;
        string[] saveFiles = null;
        try 
        {
            saveFiles = Directory.GetFiles(saveDirPath);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("PlayerData: " + e.Message);
        }

        if (saveFiles == null)
        {
            Debug.LogError("PlayerData could not find save files in save directory!");
            return;
        }

        string lastSaveFile = null;
        int i = 0;
        while (i < saveFiles.Length)
        {
            // Make sure file is a save file by checking the suffix
            if (saveFiles[i].Contains(saveFileSuffix))
            {
                if (lastSaveFile == null)
                {
                    lastSaveFile = saveFiles[i];
                }
                else
                {
                    // Check which file was created last
                    try
                    {
                        if (File.GetCreationTime(lastSaveFile).CompareTo(File.GetCreationTime(saveFiles[i])) < 0)
                        {
                            lastSaveFile = saveFiles[i];
                        }
                    }
                    catch (FileNotFoundException e) 
                    {
                        Debug.Log("PlayerData: " + e.Message);                        
                    }
                }
            }

            ++i;
        }

        if (lastSaveFile != null)
        {
            LoadPlayerData(lastSaveFile, true);
        }
        else
        {
            Debug.LogError("PlayerData could not find save file in " + saveDirPath);
        }
    }

    /**
     * Load a specific save file.
     * @param saveFilePath Include the appropriate prefix, a timestamp, and a suffix.
     * @param absolute Whether the save file path is absolute or not.
     */
    public static void LoadPlayerData(string saveFilePath, bool absolute)
    {
        string absolutePath = (absolute) ? saveFilePath : (Application.persistentDataPath + saveFilePath);
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

    /**
     * Helper method to get relative save file path.
     * @param dt Date & time at which save file was created.
     */
    public static string GetSaveFilePath(System.DateTime dt)
    {
        return saveFilePrefix + dt.ToFileTime() + saveFileSuffix;
    }
}
