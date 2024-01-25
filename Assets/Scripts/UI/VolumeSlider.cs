using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : SliderPercentage
{
    [SerializeField] private float MinVolume = -50.0f;
    [SerializeField] private float MaxVolume = 5.0f;


    public AudioMixer mixer;
    new private void Start()
    {
        base.Start();
        mixer.SetFloat("volume", Mathf.Lerp(MinVolume, MaxVolume, value));
    }

    new public void UpdatePercentageSavePref()
    {
        base.UpdatePercentageSavePref();
        mixer.SetFloat("volume", Mathf.Lerp(MinVolume, MaxVolume, value));
    }
}
