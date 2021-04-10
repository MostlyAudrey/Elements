using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameBtn;
    public Button creditsBtn;
    public Button exitGameBtn;

    public GameObject creditsWidget;

    void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        creditsBtn.onClick.AddListener(OpenCredits);
        exitGameBtn.onClick.AddListener(ExitGame);

        // Enable mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        // Hide main menu to avoid interference with loading screen
        gameObject.SetActive(false);
        SceneLoader.Get().GoToWorld(World.GAME_WORLD);
    }

    public void OpenCredits()
    {
        if (creditsWidget != null)
        {
            creditsWidget.SetActive(!creditsWidget.activeSelf);
        }
    }

    public void ExitGame()
    {
        LoadingUtility.ExitGame();
    }
}
