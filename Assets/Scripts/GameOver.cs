using TMPro;
using UnityEngine;
using System.Collections;
public class GameOver : MonoBehaviour
{
    //count from 0 to score in half a second
    //if score > highscore, Enable highscore mark
    //count from 0 to time in half a second
    [SerializeField] TextMeshProUGUI score, time;
    [SerializeField] GameObject HighscoreMarker;
    IEnumerator ScoreCount()
    {
        int finalScore = UIManager.Instance.GetScore();
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float currentScore = Mathf.Lerp(0, finalScore, elapsed / duration);
            score.text = Mathf.FloorToInt(currentScore).ToString();
            yield return null;
        }
        score.text = finalScore.ToString();
        Debug.Log(finalScore);
    }
    void SCount()
    {
        StartCoroutine(nameof(ScoreCount));
    }
    void TCount()
    {
        StartCoroutine(nameof(TimerCount));
    }
    void HighScore()
    {
        if(UIManager.Instance.GetScore() > PlayerPrefs.GetInt("Highscore"))
        {
            HighscoreMarker.SetActive(true);
        }
    }

    IEnumerator TimerCount()
    {
        int finalTime = (int)UIManager.Instance.GetTime();
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float currentTime = Mathf.Lerp(0, finalTime, elapsed / duration);
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            time.text = $"{minutes:00}:{seconds:00}";
            yield return null;
        }

        int finalMinutes = Mathf.FloorToInt(finalTime / 60);
        int finalSeconds = finalTime % 60;
        time.text = $"{finalMinutes:00}:{finalSeconds:00}";
    }
}