using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu2 : MonoBehaviour
{
    public GameObject menuRoot;
    public bool animateMenu = true;
    public GameObject animRoot;
    public float animDuration = 0.5f;
    public Button resumeButton;
    public Button saveButton;
    public Button loadFromSaveButton;
    public Button exitButton;

    static private bool loadingFromSave = false;

    private Vector3 posBeforeAnim;

    void Start()
    {
        _LoadLastSave();

        SetPauseMenuActivation(false, false);

        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(Save);
        loadFromSaveButton.onClick.AddListener(LoadLastSave);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        // Lock cursor if player clicks screen and menu is closed
        if (!menuRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Unlock cursor if player presses escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Check player input for opening/closing menu
        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
            || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {
            SetPauseMenuActivation(!menuRoot.activeSelf, animateMenu);
        }
    }
    
    void SetPauseMenuActivation(bool active, bool animate)
    {
        if (active)
        {
            menuRoot.SetActive(true);
            Time.timeScale = 0f;
               
            EventSystem.current.SetSelectedGameObject(null);

            if (animate)
            {
                _OpenMenuAnimation();
            }
            else
            {
                _OnMenuOpened();
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (animate)
            {
                _CloseMenuAnimation();
            }
            else
            {
                _OnMenuClosed();
            }
        }
    }

    private void _OpenMenuAnimation()
    {
        //Move obj from left off screen
        Vector3 endPos = animRoot.transform.position;
        Vector3 startPos = new Vector3(endPos.x - Screen.width, endPos.y, endPos.z);
        
        animRoot.transform.SetPositionAndRotation(startPos, animRoot.transform.rotation);
        LeanTween.moveX(animRoot, endPos.x, animDuration).setIgnoreTimeScale(true).setOnComplete(
            () => _OnMenuOpened()
        );
    }

    private void _OnMenuOpened()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void _CloseMenuAnimation()
    {
        //Move obj to left off screen
        posBeforeAnim = animRoot.transform.position;
        LeanTween.moveX(animRoot, posBeforeAnim.x - Screen.width, animDuration).setIgnoreTimeScale(true).setOnComplete(
            () => 
            {
                //Reset animation
                animRoot.transform.SetPositionAndRotation(posBeforeAnim, animRoot.transform.rotation);
                _OnMenuClosed();
            }
        );
    }

    private void _OnMenuClosed()
    {
        menuRoot.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        SetPauseMenuActivation(false, animateMenu);
    }

    public void Save()
    {
        RootMotionControlScript playerRootMotionControl = FindObjectOfType<RootMotionControlScript>();
        if (playerRootMotionControl != null)
        {
            SaveSystem.SavePlayerData(playerRootMotionControl);
        }
        else
        {
            Debug.LogError("PauseMenu could not find RootMotionControlScript object");
        }
    }

    public void LoadLastSave()
    {
        //Reload current scene
        loadingFromSave = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single); //Changes scene at end of frame
    }

    private void _LoadLastSave()
    {
        if (loadingFromSave)
        {
            loadingFromSave = false;

            PlayerData data = SaveSystem.LoadPlayerData();

            RootMotionControlScript playerRootMotionControl = FindObjectOfType<RootMotionControlScript>();
            if (playerRootMotionControl != null)
            {
                playerRootMotionControl.LoadLocation(data);
            }
            else
            {
                Debug.LogError("PauseMenu could not find RootMotionControlScript object");
            }

            QuestManager.LoadQuestPhases(data);
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
