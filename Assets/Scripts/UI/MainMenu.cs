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
    }

    public void StartGame()
    {
        LoadingUtility.GoToWorld(World.GAME_WORLD);
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
