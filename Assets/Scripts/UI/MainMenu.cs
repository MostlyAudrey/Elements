using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startGameBtn;
    public Button creditsBtn;
    public Button exitGameBtn;

    public int startLevelIndex = 0;

    public GameObject creditsWidget;

    void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        creditsBtn.onClick.AddListener(OpenCredits);
        exitGameBtn.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startLevelIndex, LoadSceneMode.Single);
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
#if UNITY_STANDALONE
        Application.Quit();
#endif
 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
