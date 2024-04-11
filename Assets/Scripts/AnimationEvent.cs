using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class AnimationEvent : MonoBehaviour
{
    [SerializeField] Image slider;
    public float _speedUp;
    public void StartCar()
    {
        AutoDrive[] temp = FindObjectsOfType<AutoDrive>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].canMove = true;
        }
    }
    private void Update()
    {
        AutoDrive[] temp = FindObjectsOfType<AutoDrive>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i]._speedUp = _speedUp;
        }
    }
    public void LoadingBar()
    {
        StartCoroutine("Loading");
        Debug.Log("Loading");
    }
    IEnumerator Loading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (operation.isDone)
        {
            slider.fillAmount = operation.progress / .9f;
            yield return null;
        }
    }
}