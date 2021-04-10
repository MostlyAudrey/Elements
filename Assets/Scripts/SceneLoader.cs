using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Singleton class for handling the process of going from one scene
 * to another. Uses a loading screen and asynchronous loading to disguise stuttering.
 *
 * By Aneet Nadella
 */
public class SceneLoader : MonoBehaviour
{
    // Make sure only one instance exists in scene
    private static SceneLoader instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static SceneLoader Get()
    {
        if (instance == null)
        {
            Debug.LogError("SceneLoader does not exist within scene");
        }
        return instance;
    }

    [Tooltip("Prefab of loading screen")]
    public GameObject loadingScreenPrefab;
    private LoadingScreen loadingScreen = null;

    private World worldToLoad;

    /**
     * Call to unload current world and load desired world.
     * @param world Desired world.
     */
    public void GoToWorld(World world)
    {
        // Check if already loading a world
        if (loadingScreen != null)
        {
            return;
        }

        Debug.Log("Go to world: " + world);
        // Create loading screen
        loadingScreen = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreen>();
        loadingScreen.OnFadeIn += StartAsyncLoading;

        worldToLoad = world;
    }

    /**
     * Start async loading once loading screen fades in
     * and initialize loading screen.
     */
    private void StartAsyncLoading()
    {
        AsyncOperation loadOperation = LoadingUtility.AsyncGoToWorld(worldToLoad);
        loadingScreen.Init(loadOperation);
    }

    /**
     * Call to load current world again asynchronously with loading screen.
     */
    public void ReloadCurrentWorld()
    {
        GoToWorld((World) SceneManager.GetActiveScene().buildIndex);
        Debug.Log((World) SceneManager.GetActiveScene().buildIndex);
    }
}
