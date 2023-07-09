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
        if (isTimerRunning && GameData.inGame)
        {
            if(timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                if (displayTimer)
                {
                    timerText.SetText(TimeToString());
                }
				if ((int)timeRemaining % 300 == 0)
				{
					FindObjectOfType<FashionShow>().MakeFashionShowAvailable();
				}
				if ((int)timeRemaining % 900 == 0)
				{
					FindObjectOfType<KaijuStats>().TriggerEvolution();
				}
			}
            else
            {
                StopTimer();
            }
        }
    }

    private string TimeToString() => (Mathf.FloorToInt(timeRemaining / 60)).ToString() + ":" + (Mathf.FloorToInt(timeRemaining % 60)).ToString("00");

    public void InitializeTimer(float time)
    {
        timeRemaining = time;
        timerText.SetText(TimeToString());
    }

    public void StartTimer()
    {
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
