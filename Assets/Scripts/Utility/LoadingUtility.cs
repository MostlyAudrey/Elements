using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Enum for every scene in the build.
 * Underlying integer value should be the scene's build index.
 */
public enum World : int
{
    MAIN_MENU = 0,
    GAME_WORLD = 1
}

public static class LoadingUtility
{
    public static void GoToWorld(World world)
    {
        SceneManager.LoadScene((int) world, LoadSceneMode.Single);
    }

    public static AsyncOperation AsyncGoToWorld(World world)
    {
        return SceneManager.LoadSceneAsync((int) world, LoadSceneMode.Single);
    }

    public static void AddWorld(World world)
    {
        SceneManager.LoadScene((int) world, LoadSceneMode.Additive);
    }

    public static void ReloadCurrentWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public static void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
