using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggeredTimer : MonoBehaviour
{

    [SerializeField] private float timeRemaining;
    public bool isTimerRunning = false;
    public bool displayTimer;
    public TMP_Text timerText;


    void Update()
    {
        if (isTimerRunning)
        {
            if(timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                if (displayTimer)
                {
                    timerText.SetText((Mathf.FloorToInt(timeRemaining % 60)).ToString());
                }
            }
            else
            {
                StopTimer();
            }
        }
    }


    public void StartTimer(float time)
    {
        timeRemaining = time;
        isTimerRunning = true;
    }


    public void StopTimer()
    {
        isTimerRunning = false;
        timeRemaining = 0;
        if (displayTimer)
        {
            timerText.SetText("0");
        }
    }
}
