using TMPro;
using UnityEngine;

public class FinishLevelHandler : MonoBehaviour
{

    TimerScript theTimer;
    TMP_Text textDisplay;

    bool good;
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("EndText").activeInHierarchy)
        {
            Debug.Log("EndText was found");
            good = true;
            theTimer = GameObject.FindGameObjectWithTag("EndText").GetComponent<TimerScript>();
            textDisplay = GameObject.FindGameObjectWithTag("EndText").GetComponentsInChildren<TMP_Text>()[1];
            textDisplay.enabled = false;
        } else Debug.Log("EndText was NOT found");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayEndScreen();
        }
    }

    private void DisplayEndScreen()
    {
        if (good)
        {
            textDisplay.enabled = true;
            string endText = "Your Time: " + theTimer.GetTime();
            textDisplay.text = endText;
            good = false;
        }
        else {
            Debug.LogWarning("Reached End but no display has been set or player is still moving after finish, fix that probably : ) ");
        }

    }

}
