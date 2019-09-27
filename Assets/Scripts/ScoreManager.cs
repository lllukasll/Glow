using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    // Game Active
    public Text activeBestScore;
    public Text activeCurrentScore;
    public Text activeCurrentScoreMessage;

    // Game Over
    public Text overBestScore;
    public Text overCurrentScore;
    public Text overCurrentScoreMessage;

    private int currentScore;
    private int bestScore;

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
        bestScore = PlayerPrefs.GetInt("highscore", bestScore);

        activeBestScore.text = bestScore.ToString();
        overBestScore.text = bestScore.ToString();

        ResetCurrentScore();
    }

    public void SetScoreMessageToActive()
    {
        overCurrentScoreMessage.text = "THAT'S JUST BAD";
        activeCurrentScoreMessage.text = "STARTED";
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;

        activeCurrentScore.text = "";
        overCurrentScore.text = "0";
        activeCurrentScoreMessage.text = "TOUCH TO START";
    }

    public void AddPoint()
    {
        currentScore++;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            activeBestScore.text = bestScore.ToString();
            overBestScore.text = bestScore.ToString();
            PlayerPrefs.SetInt("highscore", bestScore);
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        activeCurrentScore.text = currentScore.ToString();
        overCurrentScore.text = currentScore.ToString();

        if (currentScore == 1) { StartCoroutine(SetScoreMessage("EHM... THAT'S OK")); }
        if (currentScore == 5) { StartCoroutine(SetScoreMessage("YOU'RE DOING FINE")); }
        if (currentScore == 10) { StartCoroutine(SetScoreMessage("THAT'S PRETTY GOOD")); }
        if (currentScore == 15) { StartCoroutine(SetScoreMessage("YOU'RE GREATE")); }
        if (currentScore == 20) { StartCoroutine(SetScoreMessage("AWESOME SCORE")); }
        if (currentScore == 25) { StartCoroutine(SetScoreMessage("THIS IS AMAZING")); }
        if (currentScore == 30) { StartCoroutine(SetScoreMessage("PERFECT")); }
        if (currentScore == 45) { StartCoroutine(SetScoreMessage("IMPOSIBLE !")); }
        if (currentScore == 50) { StartCoroutine(SetScoreMessage("THAT'S EPIC !!!")); }
    }

    private IEnumerator SetScoreMessage(string message)
    {
        overCurrentScoreMessage.text = message;
        activeCurrentScoreMessage.text = message;

        yield return new WaitForSeconds(1);
        activeCurrentScoreMessage.text = "";
    }
}
