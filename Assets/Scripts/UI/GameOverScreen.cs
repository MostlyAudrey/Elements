using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Button playAgainBtn;
    public Button quit2MenuBtn;

    public int startLevelIndex = 0;
    public int mainMenuLevelIndex = 0;

    public CanvasGroup groupToFadeIn;
    public float fadeInDuration = 2f;

    void Start()
    {
        playAgainBtn.onClick.AddListener(PlayAgain);
        quit2MenuBtn.onClick.AddListener(QuitToMenu);

        EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 0f;

        //Start fade in
        groupToFadeIn.interactable = false;
        LeanTween.value(gameObject, UpdateFadeAlpha, 0f, 1f, fadeInDuration).setIgnoreTimeScale(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(startLevelIndex, LoadSceneMode.Single);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(mainMenuLevelIndex, LoadSceneMode.Single);
    }

    private void UpdateFadeAlpha(float val)
    {
        groupToFadeIn.alpha = val;
        if (val == 1f)
        {
            groupToFadeIn.interactable = true;
        }
    }
}
