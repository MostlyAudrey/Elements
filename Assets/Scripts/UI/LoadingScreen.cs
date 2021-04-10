using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Persistent loading screen for asynchronous loading between scenes.
 * Fades in on its own. Make sure to call Init function to start updating the
 * loading progress bar and to enable the loading screen to fade out when ready.
 * 
 * By Aneet Nadella
 */
public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private ProgressBar loadingProgressBar;
    [SerializeField]
    private CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    public UnityAction OnFadeIn;
    public UnityAction OnFadeOut;

    private AsyncOperation loadOperation = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        OnFadeOut += DestroyLoadingScreen;
    }

    void Start()
    {
        loadingProgressBar.SetPercentFill(0f);
        // Start fade in
        fadeGroup.alpha = 0f;
        fadeGroup.LeanAlpha(1f, fadeDuration)
        .setIgnoreTimeScale(true)
        .setOnComplete(CallOnFadeIn);
    }

    public void Init(AsyncOperation loadOperation)
    {
        this.loadOperation = loadOperation;
    }

    void Update()
    {
        // Check if Init has been called
        if (loadOperation != null)
        {
            // Update loading progress bar
            loadingProgressBar.SetPercentFill(loadOperation.progress);

            // Start fade out if appropriate
            if (loadOperation.isDone)
            {
                fadeGroup.LeanAlpha(0f, fadeDuration)
                //.setIgnoreTimeScale(true)
                .setOnComplete(CallOnFadeOut);
            }
        }
    }

    private void DestroyLoadingScreen()
    {
        Destroy(gameObject);
    }

    // Wrapper functions to call UnityAction delegates since
    // they are not of System.Action type
    private void CallOnFadeIn() { OnFadeIn(); }
    private void CallOnFadeOut() { OnFadeOut(); }
}
