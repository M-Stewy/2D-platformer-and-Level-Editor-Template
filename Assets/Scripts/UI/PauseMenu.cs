using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    private void Awake()
    {   if (instance == null) instance = this; 
        else Destroy(this);
    }

    [SerializeField] GameObject MenuHolder;
    //[SerializeField] GameObject[] EditorMenu;

    public bool isPaused;

    [SerializeField] UnityEvent Paused;
    [SerializeField] UnityEvent UnPaused;
    private void Start()
    {
        if(!SceneManager.GetSceneByName("AlwaysActive").isLoaded)
        {
            SceneManager.LoadSceneAsync("AlwaysActive", LoadSceneMode.Additive);
        }
    }
    public void OnPressPause(InputAction.CallbackContext context)
    {
        if (context.started) { 
            
            PauseStuff();
        }
    }

    public void ResumeButton()
    {
        PauseStuff();
    }

    public void QuitToMainButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync(transform.gameObject.scene);
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }

    public void OpenSettings()
    {
        SettingsMenu.instance.OpenSettingsMenu();
    }


    void PauseStuff()
    {
        if(isPaused)
        {
            if (SettingsMenu.instance.inSettings) return;

            UnPaused.Invoke();
            isPaused = false;
            Time.timeScale = 1.0f;
            MenuHolder.SetActive(false);
            /*foreach(GameObject edit in EditorMenu)
                edit.SetActive(false);*/
        }
        else if(!isPaused)
        {
            Paused.Invoke();
            isPaused = true;
            Time.timeScale = 0.0f;
            MenuHolder.SetActive(true);
            /*
            if(DataStorage.Instance.isEditing)
            {
                foreach (GameObject edit in EditorMenu)
                    edit.SetActive(true);
            }*/
        }
    }

}
