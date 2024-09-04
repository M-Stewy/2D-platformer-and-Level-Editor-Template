using System.Collections;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private int timerInSeconds;
    [SerializeField] private TMP_Text timerText;
    private int hours;
    private int minutes;
    [SerializeField] private int seconds;

    private int seconds10s;
    private int seconds1s;

    private int minutes10s;
    private int minutes1s;

    private int secondsLeft;

    [SerializeField] bool countingUp;
    [HideInInspector]
    public bool ShouldBeCounting = true;

    [HideInInspector]
    private string timeStrng;

    private void Start()
    {
        if (!countingUp)
        {
            secondsLeft = timerInSeconds;
            StartCoroutine(CountDown(timerInSeconds));
        }
        else
        {
            secondsLeft = 0;
            StartCoroutine(CountUp());
        }
    }

    private void Update()
    {
        SetSprites();
       
    }

   
    IEnumerator CountDown(int timer)
    {
        for (int i = timer; i > 0; i--)
        {
            secondsLeft--;

            hours = Mathf.FloorToInt((secondsLeft / 60) / 60);
            minutes = Mathf.FloorToInt(secondsLeft / 60);
            seconds = Mathf.FloorToInt(secondsLeft % 60);

            seconds10s = Mathf.FloorToInt(seconds / 10);
            seconds1s = Mathf.FloorToInt(seconds % 10);

            minutes1s = Mathf.FloorToInt((secondsLeft / 60) % 10);
            minutes10s = Mathf.FloorToInt((secondsLeft / 60) / 10);
            yield return new WaitForSeconds(1);

        }
    }
    IEnumerator CountUp()
    {
        while (countingUp)
        {

            secondsLeft++;

            hours = Mathf.FloorToInt((secondsLeft / 60) / 60);
            minutes = Mathf.FloorToInt(secondsLeft / 60);
            seconds = Mathf.FloorToInt(secondsLeft % 60);

            seconds10s = Mathf.FloorToInt(seconds / 10);
            seconds1s = Mathf.FloorToInt(seconds % 10);

            minutes10s = Mathf.FloorToInt((secondsLeft / 60) / 10);
            minutes1s = Mathf.FloorToInt((secondsLeft / 60) % 10);
            yield return new WaitForSeconds(1);
            
        }
    }

    private void SetSprites()
    {
        timeStrng = hours.ToString() + ':' + minutes10s.ToString() + minutes1s.ToString() + ":" + seconds10s.ToString() + seconds1s.ToString();
        timerText.text = timeStrng;
    }

    public void SetSecsToZero()
    {
        secondsLeft = 0;
    }

    public string GetTime()
    {
        return timeStrng;
    }
}
