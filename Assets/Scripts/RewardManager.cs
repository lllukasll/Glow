using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    public int gamesToAdd;
    public float waitingTime;
    public Text activeLeftGamesCount;
    public Text overLeftGamesCount;
    public Text timerText;

    private int leftGamesCount;

    private float timer;
    private bool canCount;
    private bool doOnce;

    private DateTime nextGamesAddTime;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int hasPlayed = PlayerPrefs.GetInt("HasPlayed");

        if (hasPlayed == 0)
        {
            // App Run First Time
            PlayerPrefs.SetInt("HasPlayed", 1);
            PlayerPrefs.SetInt("LeftGamesCount", gamesToAdd);
            leftGamesCount = gamesToAdd;
        }
        else
        {
            // Not First Time
            leftGamesCount = PlayerPrefs.GetInt("LeftGamesCount");
        }

        activeLeftGamesCount.text = leftGamesCount.ToString();
        overLeftGamesCount.text = leftGamesCount.ToString();
        timer = waitingTime;
        SetTimerText(0);
        canCount = false;
        doOnce = true;

        if (PlayerPrefs.GetString("NextGamesAddTime") != "")
        {
            var addGamesDateTime = Convert.ToDateTime(PlayerPrefs.GetString("NextGamesAddTime"));
            if (DateTime.Now > addGamesDateTime)
            {
                AddGames();
            }
            else
            {
                timer = (float)(addGamesDateTime - DateTime.Now).TotalSeconds;
                SetTimerText(timer);
                canCount = true;
                doOnce = false;
            }
        }
    }

    public void AddGames(int count = 5)
    {
        canCount = false;
        doOnce = true;
        timer = waitingTime;
        SetTimerText(0);

        leftGamesCount += count;
        PlayerPrefs.SetInt("LeftGamesCount", leftGamesCount);
        PlayerPrefs.SetString("NextGamesAddTime", "");
        activeLeftGamesCount.text = leftGamesCount.ToString();
        overLeftGamesCount.text = leftGamesCount.ToString();
    }

    public void RemoveGame()
    {
        if(leftGamesCount > 0)
        {
            leftGamesCount -= 1;
            PlayerPrefs.SetInt("LeftGamesCount", leftGamesCount);
            activeLeftGamesCount.text = leftGamesCount.ToString();
            overLeftGamesCount.text = leftGamesCount.ToString();
            
            if (leftGamesCount <= 0)
            {
                nextGamesAddTime = DateTime.Now.AddSeconds(waitingTime);
                PlayerPrefs.SetString("NextGamesAddTime", nextGamesAddTime.ToString());
                StartTimer();
            }

            GameManager.instance.Continue();
        }
    }

    public int GetGamesLeft()
    {
        return leftGamesCount;
    }

    private void StartTimer()
    {
        timer = waitingTime;
        canCount = true;
        doOnce = false;
    }

    void Update()
    {
        if(timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            int timerInt = (int)timer;
            SetTimerText(timer);
        }
        else if (timer <= 0.0f && !doOnce)
        {
            AddGames();

            nextGamesAddTime = DateTime.MinValue;
            canCount = false;
            doOnce = true;
            SetTimerText(0);
            timer = 0.0f;
        }
    }

    private void SetTimerText(float waitingTime)
    {
        if(waitingTime == 0)
        {
            timerText.text = "";
            return;
        }

        int minutes = (int) (waitingTime / 60);
        int seconds = (int) (waitingTime % 60);

        string timerToShow = "";

        if(minutes <= 9 )
        {
            timerToShow += "0" + minutes;
        }
        else
        {
            timerToShow += minutes;
        }

        timerToShow += ":";

        if(seconds <= 9)
        {
            timerToShow += "0" + seconds;
        }
        else
        {
            timerToShow += seconds;
        }

        timerText.text = timerToShow;
    }
}
