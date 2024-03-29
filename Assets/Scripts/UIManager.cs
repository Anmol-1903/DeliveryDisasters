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

    [SerializeField] GameObject _pauseMenu;

    public bool IsPaused()
    {
        return isPaused;
    }
    private void Awake()
    {
        _instance = this;
        isPaused = false;
    }
    public void Pause()
    {
        isPaused = true;
        _pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        isPaused = false;
        _pauseMenu.SetActive(false);
    }
}