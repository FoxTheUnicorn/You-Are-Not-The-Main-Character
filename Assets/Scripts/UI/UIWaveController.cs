using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWaveController : MonoBehaviour
{
    public float MinWidth = 10.0f;
    public float MaxWidth = 160.0f;

    public RectTransform FillImage;
    public RectTransform BackgroundImage;

    public TMP_Text WaveText;
    public TMP_Text EnemyCountText;

    private Vector2 FillImageSize;
    private int WaveEnemyCount;

    public void InactiveWave()
    {
        FillImage.gameObject.SetActive(false);
        EnemyCountText.gameObject.SetActive(false);
        WaveText.text = "DOWNTIME";
    }

    public void Debug1()
    {
        StartNewWave(20, 50);
    }    
    
    public void Debug2()
    {
        UpdateData(25);
    }

    public void UpdateData(int RemainingEnemies)
    {
        EnemyCountText.text = RemainingEnemies + "/" + WaveEnemyCount;
        AdjustEnemyRemainingBar(RemainingEnemies);
    }

    public void StartNewWave(int Wave, int EnemyCount) {
        WaveEnemyCount = EnemyCount;

        FillImage.gameObject.SetActive(true);
        EnemyCountText.gameObject.SetActive(true);

        WaveText.text = "WAVE " + Wave;
        UpdateData(EnemyCount);
    }

    private void AdjustEnemyRemainingBar(int RemainingEnemies)
    {
        FillImage.gameObject.SetActive(true);
        Vector2 FillImageSize = FillImage.sizeDelta;
        FillImageSize.x = Mathf.Lerp(MinWidth, MaxWidth, ((float) RemainingEnemies) / WaveEnemyCount);
        FillImage.sizeDelta = FillImageSize;
    }
}
