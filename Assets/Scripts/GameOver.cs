using UnityEngine;
public class GameOver : MonoBehaviour
{
    public void MainMenu()
    {
        StartCoroutine(UIManager.Instance.LoadingScreen("MainMenu"));
    }
    public void StartRestarting()
    {
        GetComponent<Animator>().SetTrigger("GameOver");
    }
    public void Restart()
    {
        StartCoroutine(UIManager.Instance.LoadingScreen("Game"));
    }
}