using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromSaveWidget : MonoBehaviour
{
    public Transform saveListRoot;
    public GameObject saveListItemPrefab;
    public string newGameText = "NEW GAME";

    // Populate list of save file items
    void Awake()
    {
        CreateNewGameItem();

        List<string> saveFiles = SaveUtility.GetAllSaveFiles();
        if (saveFiles == null)
        {
            Debug.Log("LoadFromSaveWidget: No save files found");
            return;
        }

        List<PlayerData> dataObjs = new List<PlayerData>();
        for (int i = 0; i < saveFiles.Count; ++i)
        {
            dataObjs.Add(SaveUtility.LoadPlayerData(saveFiles[i], true));
        }
        dataObjs.Sort((a,b) => a.CompareTo(b));

        foreach (PlayerData data in dataObjs)
        {
            SaveFileItem saveFileItem = Instantiate(saveListItemPrefab, saveListRoot).GetComponent<SaveFileItem>();
            saveFileItem.Init(data.GetDateTime(), this);
        }
    }

    /**
     * Load save file corresponding with given save file item.
     */
    public void OnSaveFileSelected(SaveFileItem saveFileItm)
    {
        // Close this widget
        gameObject.SetActive(false);

        // if saveFileItm is new-game item
        if (saveFileItm.date.text.Equals(newGameText))
        {
            SceneLoader.Get().GoToWorld(World.GAME_WORLD);
            return;
        }

        string saveFilePath = SaveUtility.GetSaveFilePath(saveFileItm.dateTime, true);
        Debug.Log("OnSaveFileSelected: " + saveFilePath);
        SceneLoader.Get().LoadGameWorldFromSave(saveFilePath);
    }

    // // Automatically go to game world if no save file items exist
    // void OnEnable()
    // {
    //     if (saveListRoot.childCount == 0)
    //     {
    //         SceneLoader.Get().GoToWorld(World.GAME_WORLD);
    //         // Close this widget
    //         gameObject.SetActive(false);
    //     }
    // }

    /**
     * Helper function to create save file item that creates new game
     * instead of loading from save file.
     */
    private void CreateNewGameItem()
    {
        SaveFileItem newGameItm = Instantiate(saveListItemPrefab, saveListRoot).GetComponent<SaveFileItem>();
        newGameItm.Init(System.DateTime.MinValue, this);
        newGameItm.date.text = newGameText;
        newGameItm.time.text = "";
        newGameItm.time.gameObject.SetActive(false);
    }
}
