using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customize : MonoBehaviour
{
    [Header("All Materials")]
    [SerializeField] Material[] _Primary;
    [SerializeField] Material[] _Secondary;
    [SerializeField] Material[] _Rims;

    [Header("Customize Panel")]
    [SerializeField] Slider Hue;
    [SerializeField] Slider Sat;
    [SerializeField] Slider Ver;
    [SerializeField] Slider Met;
    [SerializeField] Slider Glo;

    [Header("SliderBG")]
    [SerializeField] Image[] bgs;

    [Header("Other")]
    [SerializeField] TextMeshProUGUI MaterialText;

    int selectedMaterial = 0;
    float[] H, S, V, M, G;

    private void Awake()
    {
        H = new float[3];
        S = new float[3];
        V = new float[3];
        M = new float[3];
        G = new float[3];

        for (int i = 0; i < 3; i++)
        {
            H[i] = PlayerPrefs.GetFloat("H" + i.ToString(), 0);
            S[i] = PlayerPrefs.GetFloat("S" + i.ToString(), 0);
            V[i] = PlayerPrefs.GetFloat("V" + i.ToString(), 1);
            M[i] = PlayerPrefs.GetFloat("M" + i.ToString(), .5f);
            G[i] = PlayerPrefs.GetFloat("G" + i.ToString(), .5f);
        }
    }
    private void Start()
    {
        TextUpdater();
    }
    public void SetMaterial()                //  Materials are updated here
    {
        for (int i = 0; i < _Primary.Length; i++)
        {
            _Primary[i].color = Color.HSVToRGB(H[0], S[0], V[0]);
            _Primary[i].SetFloat("_Metallic", M[0]);
            _Primary[i].SetFloat("_Smoothness", G[0]);
        }
        for (int i = 0; i < _Secondary.Length; i++)
        {
            _Secondary[i].color = Color.HSVToRGB(H[1], S[1], V[1]);
            _Secondary[i].SetFloat("_Metallic", M[1]);
            _Secondary[i].SetFloat("_Smoothness", G[1]);
        }
        for (int i = 0; i < _Rims.Length; i++)
        {
            _Rims[i].color = Color.HSVToRGB(H[2], S[2], V[2]);
            _Rims[i].SetFloat("_Metallic", M[2]);
            _Rims[i].SetFloat("_Smoothness", G[2]);
        }
    }
    public void NextThingToPaint()
    {
        selectedMaterial = (selectedMaterial + 1) % 3;
        SetSliders();
        TextUpdater();
    }
    void TextUpdater()
    {
        switch (selectedMaterial)
        {
            case 0:
                MaterialText.text = "Primary";
                break;
            case 1:
                MaterialText.text = "Secondary";
                break;
            case 2:
                MaterialText.text = "Rims";
                break;
        }
    }
    public void PrevThingToPaint()
    {
        selectedMaterial = (selectedMaterial - 1) < 0 ? 2 : selectedMaterial - 1;
        SetSliders();
        TextUpdater();
    }
    public void HueController(float _val)
    {
        H[selectedMaterial] = _val;
        PlayerPrefs.SetFloat("H" + selectedMaterial.ToString(), _val);
        for (int i = 0; i < bgs.Length; i++)
        {
            bgs[i].color = Color.HSVToRGB(_val, 1, 1);
        }
        SetMaterial();
    }
    public void SaturationController(float _val)
    {
        S[selectedMaterial] = _val;
        PlayerPrefs.SetFloat("S" + selectedMaterial.ToString(), _val);
        SetMaterial();
    }
    public void BrightnessController(float _val)
    {
        V[selectedMaterial] = _val;
        PlayerPrefs.SetFloat("V" + selectedMaterial.ToString(), _val);
        SetMaterial();
    }
    public void MetallicController(float _val)
    {
        M[selectedMaterial] = _val;
        PlayerPrefs.SetFloat("M" + selectedMaterial.ToString(), _val);
        SetMaterial();
    }
    public void SmoothnessController(float _val)
    {
        G[selectedMaterial] = _val;
        PlayerPrefs.SetFloat("G" + selectedMaterial.ToString(), _val);
        SetMaterial();
    }
    public void SetSliders()
    {
        for (int i = 0; i < 3; i++)
        {
            Hue.value = PlayerPrefs.GetFloat("H" + selectedMaterial.ToString(), 0);
            Sat.value = PlayerPrefs.GetFloat("S" + selectedMaterial.ToString(), 0);
            Ver.value = PlayerPrefs.GetFloat("V" + selectedMaterial.ToString(), 1);
            Met.value = PlayerPrefs.GetFloat("M" + selectedMaterial.ToString(), .5f);
            Glo.value = PlayerPrefs.GetFloat("G" + selectedMaterial.ToString(), .5f);
        }
        for (int i = 0; i < bgs.Length; i++)
        {
            bgs[i].color = Color.HSVToRGB(Hue.value, 1, 1);
        }
    }
}