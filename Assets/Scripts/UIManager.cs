using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    [SerializeField] GameObject _pauseMenu, gameOverPanel;
    [SerializeField] GameObject _restartButton;

    bool gameOver;
    float time, c;
    int score, highscore;

    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return c;
    }
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
        time = 1f;
        scoreText.text = $"Score: {score}";
        highscoreText.text = $"Highscore: {highscore}";
    }
    private void OnDestroy()
    {
        DeliveryManager.OnTimeUpdate -= UpdateScore;
    }
    public void UpdateTime(int amt)
    {
        time += amt;
    }
    private void UpdateScore(int pts)
    {
        score += pts;
        scoreText.text = $"Score: {score}";
        if(score > highscore)
        {
            UpdateHighscore();
        }
    }

    private void Update()
    {
        c += Time.deltaTime;
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
        highscoreText.text = $"Highscore: {highscore}";
        PlayerPrefs.SetInt("Highscore", highscore);
    }
    public void Timer()
    {
        if (time <= 0)
        {
            GameOver();
            timeText.text = "00:00";
        }
        else
        {
            time -= Time.deltaTime;
            timeText.text = $"{Mathf.FloorToInt(time / 60).ToString("00")}:{(time % 60).ToString("00")}";
        }
    }
    void GameOver()
    {
        if (gameOver)
            return;
        gameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(_restartButton);

    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}