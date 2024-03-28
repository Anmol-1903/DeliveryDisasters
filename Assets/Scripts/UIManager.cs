using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    int _currentScore, _highScore;
    float _timer = 91f, _counter = 0f;
    Animator _anim;
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager not initialized");
            return _instance;
        }
    }

    [SerializeField] public GameObject _pauseMenu;
    [SerializeField] public GameObject _gameOverScreen;
    public Animator anim;
    public bool isPaused = false;
    public bool pauseLock = false;
    public bool gameOver = false;

    [SerializeField] TextMeshProUGUI _currentScoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;
    [SerializeField] TextMeshProUGUI _timerText;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (_highScoreText)
            _highScoreText.text = "Highscore : " + _highScore.ToString("d2");
        if (_currentScoreText)
            _currentScoreText.text = "Score : " + 00.ToString();
        _instance = this;
        _anim = GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            ResumeGame();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !pauseLock && SceneManager.GetActiveScene().buildIndex != 0)
        {
            pauseLock = true;
            if (!isPaused)
            {
                _pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else 
            {
                if (SceneManager.GetActiveScene().buildIndex != 0)
                    ResumeGame();
            }
        }
        if (!isPaused)
        {
            _counter += Time.deltaTime;
            TimeCounter();
        }
        else
        {
            AudioManager.Instance.ResetVibrations();
        }
    }
    void TimeCounter()
    {
        if (_timer <= 0)
        {
            _timer = 0f;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        if (_timerText)
            _timerText.text = ((int)(_timer / 60f)).ToString("d2") + " : " + ((int)(_timer % 60f)).ToString("d2");
    }
    public IEnumerator LoadingScreen(string _sceneName)
    {
        _anim.SetTrigger("StartLoading");
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneName);
        while (!_operation.isDone)
        {
            yield return null;
        }
    }
    public void ResumeGame()
    {
        anim.SetTrigger("Close");
    }
    public void UpdateScore(int _amount)
    {
        _currentScore += _amount;
        _currentScoreText.text = "Score : " + _currentScore.ToString("d2");
        if(_currentScore > _highScore)
        {
            _highScore = _currentScore;
            UpdateHighScore(_highScore);
        }
    }
    public void UpdateHighScore(int _amount)
    {
        _highScoreText.text = "Highscore : " + _amount.ToString("d2");
    }
    public void AddTime(float _amount)
    {
        _timer += _amount;
    }
    public float GetTime()
    {
        return _timer;
    }
    public void GameOverScreen()
    {
        gameOver = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _gameOverScreen.SetActive(true);
    }
    public int GetScore()
    {
        return _currentScore;
    }
    public float GetCounter()
    {
        return _counter;
    }
}