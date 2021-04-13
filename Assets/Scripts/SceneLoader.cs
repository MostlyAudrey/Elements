using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Singleton class for handling the process of going from one scene
 * to another. Uses a loading screen and asynchronous loading to disguise stuttering.
 * Persistent across levels.
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
            DontDestroyOnLoad(gameObject);
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

    private AsyncOperation loadOperation = null;
    private World worldToLoad;
    // Absolute path of save file
    private string saveFile = null;

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
        loadOperation = LoadingUtility.AsyncGoToWorld(worldToLoad);
        loadingScreen.Init(loadOperation);
        loadOperation.completed += OnAsyncLoadComplete;

        // Clear all UI to avoid interference with loading screen
        UICanvas.Get().ClearScreen();
        // Make sure time scale is normal
        Time.timeScale = 1f;
    }

    /**
     * Call to load current world again asynchronously with loading screen.
     */
    public void ReloadCurrentWorld()
    {
        GoToWorld((World) SceneManager.GetActiveScene().buildIndex);
    }

    private void OnAsyncLoadComplete(AsyncOperation loadOp)
    {
        // Let go of refs
        loadingScreen = null;
        loadOperation = null;

        // Load save file if necessary
        if (saveFile != null)
        {
            SaveUtility.LoadPlayerData(saveFile, true).LoadGame();
            saveFile = null;
        }
    }

    /**
     * Call to load game world with a specified save file.
     */
    public void LoadGameWorldFromSave(string saveFile)
    {
        this.saveFile = saveFile;
        GoToWorld(World.GAME_WORLD);
    }
}
