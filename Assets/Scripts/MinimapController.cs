using UnityEngine;
public class MinimapController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _playerBlip;

    float h, s, v;
    Color blipClr;
    private void Start()
    {
        h = PlayerPrefs.GetFloat("H0", 0);
        s = PlayerPrefs.GetFloat("S0", 0);
        v = PlayerPrefs.GetFloat("V0", 1);
        blipClr = Color.HSVToRGB(h, s, v);

        _playerBlip.color = blipClr;
    }
}