using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPercentage : MonoBehaviour
{
    [SerializeField] Slider mainSlider;
    [SerializeField] TMP_Text PercentageText;
    [SerializeField] string pref;
    float value = 0.0f;

    private void Start()
    {
        value = PlayerPrefs.GetFloat(pref);
        mainSlider.value = value * mainSlider.maxValue;
    }

    public void UpdatePercentage()
    {
        value = (mainSlider.value / mainSlider.maxValue);
        PercentageText.text = value * 100 + "%";
    }
    public void UpdatePercentageSavePref()
    {
        UpdatePercentage();
        PlayerPrefs.SetFloat(pref, value);
    }
}
