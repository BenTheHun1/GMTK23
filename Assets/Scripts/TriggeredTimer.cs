using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggeredTimer : MonoBehaviour
{
    [SerializeField] private float fashionShowFrequency = 300f;
    [SerializeField] private float evolutionFrequency = 900f;
    [SerializeField] private float timeRemaining;

    [SerializeField] private bool debugEndTimer;

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
				if ((int)timeRemaining % fashionShowFrequency == 0)
				{
					FindObjectOfType<FashionShow>().MakeFashionShowAvailable();
				}
				if ((int)timeRemaining % evolutionFrequency == 0)
				{
					FindObjectOfType<KaijuStats>().TriggerEvolution();
				}
			}
            else
            {
                StopTimer();
                GameManager.instance.GameEnd();
            }
        }

        if(debugEndTimer && isTimerRunning)
        {
            debugEndTimer = false;
            timeRemaining = 0;
            StopTimer();
            GameManager.instance.GameEnd();
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
            timerText.SetText("0:00");
        }
    }
}
