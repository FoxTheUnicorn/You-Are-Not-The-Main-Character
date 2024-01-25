using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    [SerializeField] RectTransform HealthTransform;
    [SerializeField] Image HealthImage;

    [SerializeField] float MaximumHeight = 274.0f;
    [SerializeField] float MinimumHeight = 12.0f;

    [SerializeField] float HealthOkay = 40.0f; //In Percent
    [SerializeField] float HealthDanger = 5.0f; //In Percent
    [SerializeField] Color HealthGoodColor; //In Percent
    [SerializeField] Color HealthOkayColor; //In Percent
    [SerializeField] Color HealthDangerColor; //In Percent

    private float MaxHealth;


    public void Start()
    {
        InitHealthBar(100);
        Invoke("Debug1", 2.5f);
        Invoke("Debug2", 5.0f);
    }

    private void Debug1() { UpdateHealthBar(30.0f); }
    private void Debug2() { UpdateHealthBar(4.0f); }

    private void UpdateColor(float value)
    {
        Color BarColor = HealthGoodColor;
        if (value < HealthOkay) BarColor = HealthOkayColor;
        if (value < HealthDanger) BarColor = HealthDangerColor;
        HealthImage.color = BarColor;
    }

    public void UpdateHealthBar(float CurrentHealth)
    {
        float value = CurrentHealth / MaxHealth;
        Vector2 HealthTransformSize = HealthTransform.sizeDelta;
        HealthTransformSize.y = Mathf.Lerp(MinimumHeight, MaximumHeight, value);
        HealthTransform.sizeDelta = HealthTransformSize;
        UpdateColor(value * 100);
    }

    public void InitHealthBar(float MaxHealth)
    {
        this.MaxHealth = MaxHealth;
    }
}
