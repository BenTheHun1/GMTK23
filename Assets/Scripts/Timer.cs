using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private DateTime now,
        countdownTime;

    //is timer running
    private bool setTimer = false;

    //assign text box here to automatically set time display
    public TMP_Text clock;

    // Start is called before the first frame update
    void Start()
    {
        //DisplayCurrentDateTime();
        //TimeSpan test = new TimeSpan(0, 0, 0, 10);
        //StartCountdown(test);
    }

    // Update is called once per frame
    void Update()
    {

        //only compares time if the timer is running
        if (setTimer)
        {
            CompareDateTime();
        }
    }

    //gets current time
    //returns DateTime for current time
    public DateTime GetDateTime()
    {
        return now = DateTime.Now;
    }

    //compares countdowntime to current time and stops countdown when they match
    public void CompareDateTime()
    {
        if (DateTime.Compare(countdownTime, GetDateTime()) <= 0)
        {
            StopCountdown();
        }
    }

    //takes a TimeSpan as a parameter
    //returns DateTime countdowntime once it is set
    //sets countdown time for game and saves it to playerprefs
    public DateTime StartCountdown(TimeSpan duration)
    {
        setTimer = true;
        countdownTime = GetDateTime().Add(duration);
        SetPlayerPrefs();
        DisplayCountdownTime();
        return countdownTime;
    }

    //stops countdown
    public void StopCountdown()
    {
        setTimer = false;
        Debug.Log("timer out");
    }

    //sets playerprefs for countdowntime and settimer bool
    public void SetPlayerPrefs()
    {
        PlayerPrefs.SetString("CountdownTime", countdownTime.ToString());
        PlayerPrefs.SetString("TimerBool", setTimer.ToString());
    }

    //displays current time on text box
    public void DisplayCurrentDateTime()
    {
        clock.SetText(DateTime.Now.ToString());
    }
    

    //displays countdown time on text box
    public void DisplayCountdownTime()
    {
        clock.SetText(countdownTime.ToString());
    }

    //gets countdown time value from playerprefs
    //to be called when game reboots
    //returns DateTime for countdown
    public DateTime GetTimeFromPrefs()
    {
        DateTime.TryParse(PlayerPrefs.GetString("CountdownTime"), out countdownTime);
        return countdownTime;
    }

    //gets settimer bool value from playerprefs
    //to be called when game reboots
    //returns timer bool
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
