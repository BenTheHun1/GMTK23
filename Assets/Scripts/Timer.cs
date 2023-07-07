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
        if (DateTime.Compare(countdownTime, GetDateTime()) >= 0)
        {
            StopCountdown();
        }
    }


    public DateTime StartCountdown(TimeSpan duration)
    {
        setTimer = true;
        return countdownTime = GetDateTime().Add(duration);
    }

    public void StopCountdown()
    {
        setTimer = false;
        Debug.Log("timer out");
    }


    public void DisplayCurrentDateTime()
    {
        Debug.Log(DateTime.Now.ToString());
    }

    public void DisplayCountdownTime()
    {
        Debug.Log(countdownTime.ToString());
    }
}
