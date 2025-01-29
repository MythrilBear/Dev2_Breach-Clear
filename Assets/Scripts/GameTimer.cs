using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 300f;
    public bool timerIsRunning = false;
    private float timeRemaining;
    public TMP_Text timerText;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //to start timer
        timeRemaining = totalTime;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {//to decrease time
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);//to update UI text
            }
            else
            {
                Debug.Log("Time's up!!!");
                timeRemaining = 0;
                timerIsRunning = false;
                TimerEnded();
            }
        } 
    }
    
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; //timer using minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);//to update the UI text
    }

    void TimerEnded()
    {
        Debug.Log("Timer has ended! GAME OVER");
        GameManager.instance.timeIsUp();
    }
}
