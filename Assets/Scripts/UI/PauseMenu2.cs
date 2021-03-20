using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu2 : MonoBehaviour
{
    public GameObject menuRoot;
    public Button resumeButton;
    public Button saveButton;
    public Button loadFromSaveButton;
    public Button exitButton;

    void Start()
    {
        SetPauseMenuActivation(false);

        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(Save);
        loadFromSaveButton.onClick.AddListener(LoadLastSave);
        exitButton.onClick.AddListener(ExitGame);
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
            Time.timeScale = 0f;
               
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resumes game time
        }
    }

    public void Resume()
    {
        SetPauseMenuActivation(false);
    }

    public void Save()
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

    public void LoadLastSave()
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

        Resume();
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
