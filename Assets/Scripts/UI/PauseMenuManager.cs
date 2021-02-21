using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject used to toggle menu activation")]
    public GameObject menuRoot;

    public QuestListDisplay m_QuestListDisplay;

    void Start()
    {
        SetPauseMenuActivation(false);
    }

    void Update()
    {
        // Check player input for opening/closing menu
        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu))
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
            Time.timeScale = 0f; // Pauses game time

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
}
