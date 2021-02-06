using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject used to toggle menu activation")]
    public GameObject menuRoot;

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

            // Refresh quest interface
            QuestListManager questListManager = GetComponentInChildren<QuestListManager>();
            if (questListManager != null)
            {
                questListManager.RefreshOngoingQuests();
            }
            else
            {
                Debug.Log("Not able to find quest list manager!");
            }
               
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resumes game time
        }
    }
}
