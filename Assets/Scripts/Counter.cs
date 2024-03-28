using TMPro;
using UnityEngine;
public class Counter : MonoBehaviour
{
    [Tooltip("Check if it is a score text, uncheck if it is timer text")]
    [SerializeField] bool isScore;
    [SerializeField] GameObject _highscoreImg;
    float t,s;
    private void Update()
    {
        Increment();
    }
    void Increment()
    {
                Debug.Log(s + " " + PlayerPrefs.GetInt("HighScore"));
        if (isScore)
        {
            if (s < UIManager.Instance.GetScore())
            {
                s += UIManager.Instance.GetScore() * Time.unscaledDeltaTime / 2f;
                gameObject.GetComponent<TextMeshProUGUI>().text = ((int)s).ToString();
            }
            else
            {
                s = UIManager.Instance.GetScore();
            }

            if ((int)s > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", (int)s);
                _highscoreImg.SetActive(true);
            }
        }
        else
        {
            if (t < UIManager.Instance.GetCounter())
            {
                t += UIManager.Instance.GetCounter() * Time.unscaledDeltaTime / 2f;
                gameObject.GetComponent<TextMeshProUGUI>().text = "in " + ((int)(t / 60f)).ToString("d2") + " : " + ((int)(t % 60f)).ToString("d2");
            }
        }
    }
}