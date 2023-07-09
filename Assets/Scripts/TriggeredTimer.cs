using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggeredTimer : MonoBehaviour
{
    [SerializeField] private float fashionShowFrequency = 300f;
    [SerializeField] private float evolutionFrequency = 900f;
	[SerializeField] private float totalTime;
    [SerializeField] private float timeRemaining;

    [SerializeField] private bool debugEndTimer;

    public bool isTimerRunning = false;
    public bool displayTimer;
    public TMP_Text timerText;

	public List<float> evolveTimes;
	public int curEvolve;
	public List<float> fashionTimes;
	public int curFashion;

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
				if ((int)timeRemaining == evolveTimes[curEvolve])
				{
					curEvolve++;
					FindObjectOfType<KaijuStats>().TriggerEvolution();
				}
				if ((int)timeRemaining == fashionTimes[curFashion])
				{
					curFashion++;
					FindObjectOfType<FashionShow>().MakeFashionShowAvailable();
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

    public void InitializeTimer()
    {
        timeRemaining = totalTime;
        timerText.SetText(TimeToString());
		float totalTime2 = totalTime;
		while (true)
		{
			evolveTimes.Add(totalTime2 - evolutionFrequency);
			totalTime2 -= evolutionFrequency;
			if (totalTime2 - evolutionFrequency < 0)
			{
				break;
			}
		}
		totalTime2 = totalTime;
		while (true)
		{
			fashionTimes.Add(totalTime2 - fashionShowFrequency);
			totalTime2 -= fashionShowFrequency;
			if (totalTime2 - fashionShowFrequency < 0)
			{
				break;
			}
		}
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
