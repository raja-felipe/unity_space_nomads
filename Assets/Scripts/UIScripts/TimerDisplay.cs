using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    // [SerializeField] private gameManagerScript gameManager;
    public float targetTime;
    private const int MINUTE_SECONDS = 60;
    private const int DOUBLE_DIGITS = 10;
    [SerializeField] private TextMeshProUGUI timerText;
    public string currTime = "00:00";
    public float seconds;
    public float timeRatio;
    public string timePercent;
    
    // Start is called before the first frame update
    void Start()
    {
        targetTime = gameManagerScript.manager.getTargetTime();
    }

    // Update is called once per frame
    void Update()
    {
        float timeLeft = targetTime - Time.timeSinceLevelLoad;
        float minutesLeft = timeLeft / MINUTE_SECONDS;
        float secondsLeft = timeLeft % MINUTE_SECONDS;
        string minutesDisplay = "";
        string secondsDisplay = "";
        
        // Convert minutes to string
        if (minutesLeft >= DOUBLE_DIGITS)
        {
            minutesDisplay = ((int)(minutesLeft)).ToString();
        }

        else
        {
            minutesDisplay = ((int)(minutesLeft)).ToString("00");
        }

        // Convert seconds to string
        if (secondsLeft >= DOUBLE_DIGITS)
        {
            secondsDisplay = ((int)(secondsLeft)).ToString();
        }

        else
        {
            secondsDisplay = ((int)(secondsLeft)).ToString("00");
        }
        
        // Now display the time
        seconds = (MINUTE_SECONDS*(int)(minutesLeft) + (int)(secondsLeft));
        timerText.text = minutesDisplay + ":" + secondsDisplay;
        currTime = timerText.text;
        timeRatio = seconds / targetTime;
        timePercent = (int)(timeRatio * 100) + "%";
        SetGlobalEndTime();
    }
    
    public void SetGlobalEndTime()
    {
        GlobalSceneManager.EndTime = currTime;
        GlobalSceneManager.EndTimeRatio = timeRatio;
        GlobalSceneManager.EndTimePercent = timePercent;
    }
}
