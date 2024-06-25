using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject MenuHolder;
    [SerializeField] GameObject[] EditorMenu;

    bool isPaused;

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
        SceneManager.UnloadSceneAsync(transform.gameObject.scene);
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }


    void PauseStuff()
    {
        if(isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            MenuHolder.SetActive(false);
            foreach(GameObject edit in EditorMenu)
                edit.SetActive(false);
        }
        else if(!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
            MenuHolder.SetActive(true);

            if(DataStorage.Instance.isEditing)
            {
                foreach (GameObject edit in EditorMenu)
                    edit.SetActive(true);
            }
        }
    }

}
