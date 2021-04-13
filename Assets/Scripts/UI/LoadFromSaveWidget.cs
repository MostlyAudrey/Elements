using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromSaveWidget : MonoBehaviour
{
    public Transform saveListRoot;
    public GameObject saveListItemPrefab;

    // Populate list of save file items
    void Awake()
    {
        List<string> saveFiles = SaveUtility.GetAllSaveFiles();
        if (saveFiles == null)
        {
            Debug.LogError("LoadFromSaveWidget: No save files found");
            return;
        }

        List<PlayerData> dataObjs = new List<PlayerData>();
        for (int i = 0; i < saveFiles.Count; ++i)
        {
            dataObjs.Add(SaveUtility.LoadPlayerData(saveFiles[i], true));
        }

        foreach (PlayerData data in dataObjs)
        {
            SaveFileItem saveFileItem = Instantiate(saveListItemPrefab, saveListRoot).GetComponent<SaveFileItem>();
            saveFileItem.Init(data.GetDateTime(), this);
        }
    }

    /**
     * Load save file corresponding with given date & time.
     * @param dt Date and time of save file to load.
     */
    public void OnSaveFileSelected(System.DateTime dt)
    {
        string saveFilePath = SaveUtility.GetSaveFilePath(dt, true);
        Debug.Log("OnSaveFileSelected: " + saveFilePath);
        SceneLoader.Get().LoadGameWorldFromSave(saveFilePath);
    }

    // Automatically go to game world if no save file items exist
    void OnEnable()
    {
        if (saveListRoot.childCount == 0)
        {
            SceneLoader.Get().GoToWorld(World.GAME_WORLD);
            // Close this widget
            gameObject.SetActive(false);
        }
    }
}
