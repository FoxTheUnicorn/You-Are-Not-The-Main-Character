using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSoundController : SoundController
{
    public void FootStep()
    {
        Play(0);
    }

    public void slash()
    {
        Play(1);
    }

}
