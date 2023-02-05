using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countdown : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        timerIsRunning = true;
        timeText = FindObjectOfType<TextMeshProUGUI>();
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }
}