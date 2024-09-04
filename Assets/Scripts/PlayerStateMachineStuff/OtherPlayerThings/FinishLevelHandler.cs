using TMPro;
using UnityEngine;

public class FinishLevelHandler : MonoBehaviour
{

    TimerScript theTimer;
    TMP_Text textDisplay;

    private void Start()
    {
        theTimer = GameObject.FindGameObjectWithTag("EndText").GetComponent<TimerScript>();
        textDisplay = GameObject.FindGameObjectWithTag("EndText").GetComponentsInChildren<TMP_Text>()[1];
        textDisplay.enabled = false;
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
        textDisplay.enabled = true;
        string endText = "Your Time: " + theTimer.GetTime();
        textDisplay.text = endText;

    }

}
