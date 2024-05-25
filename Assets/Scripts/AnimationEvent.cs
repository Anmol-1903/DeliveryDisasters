using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AnimationEvent : MonoBehaviour
{
    [SerializeField] Image slider;
    public float _speedUp;
    AsyncOperation operation;
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
        if (operation != null)
        {
            slider.fillAmount = Mathf.Lerp(0, 1, operation.progress / .9f);
            if (slider.fillAmount >= 1f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
    void Loading()
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;
    }
}