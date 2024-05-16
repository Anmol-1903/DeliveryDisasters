using System;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }
    bool isPaused;

    [SerializeField] TextMeshProUGUI timeText, scoreText, highscoreText;

    [SerializeField] GameObject _pauseMenu;

    float time;
    int score, highscore;

    public bool IsPaused()
    {
        return isPaused;
    }
    private void Awake()
    {
        _instance = this;

        DeliveryManager.OnTimeUpdate += UpdateScore;
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        isPaused = false;
        time = 91f;
        scoreText.text = $"Score: {score.ToString()}";
        highscoreText.text = $"Highscore: {highscore.ToString()}";
    }
    private void OnDestroy()
    {
        DeliveryManager.OnTimeUpdate -= UpdateScore;
    }
    private void UpdateScore(int pts)
    {
        score += pts;
        scoreText.text = $"Score: {score.ToString()}";
        if(score > highscore)
        {
            UpdateHighscore();
        }
    }

    private void Update()
    {
        Timer();
    }
    public void Pause()
    {
        isPaused = true;
        _pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UpdateHighscore()
    {
        highscore = score;
        highscoreText.text = $"Highscore: {highscore.ToString()}";
        PlayerPrefs.SetInt("Highscore", (int)highscore);
    }
    public void Timer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            //game Over
        }
        timeText.text = $"{(time / 60).ToString("00")}:{(time % 60).ToString("00")}";
    }
}