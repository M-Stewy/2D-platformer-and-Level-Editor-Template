using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject MenuHolder;
    [SerializeField] GameObject EditorMenu;

    bool isPaused;


    public void OnPressPause(InputAction.CallbackContext context)
    {
        if (context.started) { 
            
            PauseStuff();
        }
    }
    //maybe put this indivually in each scene as opposed to the always active?

    void PauseStuff()
    {
        if(isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            MenuHolder.SetActive(false);
            EditorMenu.SetActive(false);
        }
        else if(!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
            MenuHolder.SetActive(true);

            if(DataStorage.Instance.isEditing)
            {
                EditorMenu.SetActive(true);
            }
        }
    }

}
