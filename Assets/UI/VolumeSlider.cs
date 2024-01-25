using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : SliderPercentage
{
    public AudioMixer mixer;
    new private void Start()
    {
        base.Start();
        mixer.SetFloat("volume", Mathf.Lerp(-80.0f, 20.0f, value));
    }

    new public void UpdatePercentageSavePref()
    {
        base.UpdatePercentageSavePref();
        mixer.SetFloat("volume", Mathf.Lerp(-80.0f, 20.0f, value));
    }
}
