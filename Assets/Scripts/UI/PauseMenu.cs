using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("Root GameObject used to toggle menu activation")]
    public GameObject menuRoot;
    [Tooltip("Button used to resume game")]
    public Button resumeGameButton;
    [Tooltip("Button used to save current game progress")]
    public Button saveGameButton;
    [Tooltip("Button used to load game from save file")]
    public Button loadFromSaveButton;
    [Tooltip("Button used to save current game progress and subsequently exit")]
    public Button saveAndExitButton;
    [Tooltip("Button used to exit game without saving")]
    public Button exitNoSaveButton;
    public QuestListWidget m_QuestListDisplay;

    void Start()
    {
        resumeGameButton.onClick.AddListener(ResumeGame);
        saveGameButton.onClick.AddListener(SaveGame);
        loadFromSaveButton.onClick.AddListener(LoadFromSave);
        saveAndExitButton.onClick.AddListener(SaveAndExit);
        exitNoSaveButton.onClick.AddListener(ExitWithoutSaving);

        SetPauseMenuActivation(false);
    }

    void Update()
    {
        // Lock cursor if player clicks screen and menu is closed
        if (!menuRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Unlock cursor if player presses escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Check player input for opening/closing menu
        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
            || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {
            SetPauseMenuActivation(!menuRoot.activeSelf);
        }
    }

    void SetPauseMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Time.timeScale = 0f; // Pauses game time

            m_QuestListDisplay.OpenDisplay();
               
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resumes game time

            m_QuestListDisplay.CloseDisplay();
        }
    }

    void ResumeGame()
    {
        SetPauseMenuActivation(false);
    }

    void SaveGame()
    {
        RootMotionControlScript playerRootMotionControl = FindObjectOfType<RootMotionControlScript>();
        if (playerRootMotionControl != null)
        {
            SaveSystem.SavePlayerData(playerRootMotionControl);
        }
        else
        {
            Debug.LogError("PauseMenu could not find RootMotionControlScript object");
        }
    }

    void LoadFromSave()
    {
        PlayerData data = SaveSystem.LoadPlayerData();

        RootMotionControlScript playerRootMotionControl = FindObjectOfType<RootMotionControlScript>();
        if (playerRootMotionControl != null)
        {
            playerRootMotionControl.LoadLocation(data);
        }
        else
        {
            Debug.LogError("PauseMenu could not find RootMotionControlScript object");
        }

        QuestManager.LoadQuestPhases(data);

        ResumeGame();
    }

    void SaveAndExit()
    {
        SaveGame();
        ExitWithoutSaving();
    }

    void ExitWithoutSaving()
    {
        //Add exit functionality
    }
}
