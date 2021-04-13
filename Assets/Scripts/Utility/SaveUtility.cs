using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/**
 * Utility class for creating save files and accessing them.
 * Made by Aneet Nadella and Akash C.
 */
public static class SaveUtility
{
    public static string saveFilePrefix = "/player_data";
    public static string saveFileSuffix = ".elements";
    
    public static void SavePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string absolutePath = GetSaveFilePath(System.DateTime.Now, true);

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
     * Load a specific save file.
     * @param saveFilePath Include the appropriate prefix, a timestamp, and a suffix.
     * @param absolute Whether the save file path is absolute or not.
     */
    public static PlayerData LoadPlayerData(string saveFilePath, bool absolute)
    {
        string absolutePath = (absolute) ? saveFilePath : (Application.persistentDataPath + saveFilePath);
        if (File.Exists(absolutePath))
        {  
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(absolutePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found at " + absolutePath);
            return null;
        }
    }

    /**
     * Get a list of all save files.
     * @return List of absolute paths for all save files.
     */
    public static List<string> GetAllSaveFiles()
    {
        string saveDirPath = Application.persistentDataPath;
        string[] files = null;
        try
        {
            files = Directory.GetFiles(saveDirPath);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("PlayerData: " + e.Message);
        }

        if (files == null)
        {
            Debug.LogError("PlayerData could not find any files in save directory!");
            return null;
        }

        List<string> saveFiles = new List<string>();
        for (int i = 0; i < files.Length; ++i)
        {
            // Make sure file is a save file by checking the suffix
            if (files[i].Contains(saveFileSuffix))
            {
                saveFiles.Add(files[i]);
            }
        }

        return saveFiles;
    }

    /**
     * Get absolute path of last created save file.
     */
    public static string GetLastSaveFile()
    {
        List<string> saveFiles = GetAllSaveFiles();

        string lastSaveFile = null;
        for (int i = 0; i < saveFiles.Count; ++i)
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

        return lastSaveFile;
    }

    /**
     * Get the save file path from the time of the save file's creation.
     * @param dt Date & time at which save file was created.
     * @param absolute Whether return value should be absolute or relative path.
     */
    public static string GetSaveFilePath(System.DateTime dt, bool absolute)
    {
        return ((absolute) ? Application.persistentDataPath : "") + saveFilePrefix + dt.ToString("yyyyMMdd_hhmmss") + saveFileSuffix;
    }
}
