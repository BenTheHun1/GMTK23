using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private DateTime now,
        countdownTime;

    private bool setTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        DisplayCurrentDateTime();
        TimeSpan test = new TimeSpan(0, 0, 0, 10);
        StartCountdown(test);
    }

    // Update is called once per frame
    void Update()
    {
        if (setTimer)
        {
            CompareDateTime();
        }
    }


    public DateTime GetDateTime()
    {
        return now = DateTime.Now;
    }


    public void CompareDateTime()
    {
        if (DateTime.Compare(countdownTime, GetDateTime()) <= 0)
        {
            Debug.Log("prefs" + GetBoolFromPrefs());
            StopCountdown();
        }
    }


    public DateTime StartCountdown(TimeSpan duration)
    {
        setTimer = true;
        countdownTime = GetDateTime().Add(duration);
        SetPlayerPrefs();
        return countdownTime;
    }

    public void StopCountdown()
    {
        setTimer = false;
        Debug.Log("timer out");
    }


    public void SetPlayerPrefs()
    {
        PlayerPrefs.SetString("CountdownTime", countdownTime.ToString());
        PlayerPrefs.SetString("TimerBool", setTimer.ToString());
    }


    public void DisplayCurrentDateTime()
    {
        Debug.Log(DateTime.Now.ToString());
    }

    public void DisplayCountdownTime()
    {
        Debug.Log(countdownTime.ToString());
    }


    public DateTime GetTimeFromPrefs()
    {
        DateTime.TryParse(PlayerPrefs.GetString("CountdownTime"), out countdownTime);
        return countdownTime;
    }

    public bool GetBoolFromPrefs()
    {
        if (PlayerPrefs.GetString("TimerBool").Equals("false"))
        {
            setTimer = false;
        }
        else
        {
            setTimer = true;
        }
        return setTimer;
    }
}
