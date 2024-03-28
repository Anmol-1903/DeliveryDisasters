using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fps;
    [SerializeField] float[] last50Frames;
    int index;
    private void Awake()
    {
        last50Frames = new float[50];
        index = 0;
    }
    private void Update()
    {
        last50Frames[index] = Time.unscaledDeltaTime;
        index = (index + 1) % last50Frames.Length;
        fps.text = Mathf.RoundToInt(calculateFPS()).ToString();
    }
    float calculateFPS()
    {
        float total = 0;
        foreach (float frame in last50Frames)
        {
            total += frame;
        }
        return last50Frames.Length / total;
    }
}