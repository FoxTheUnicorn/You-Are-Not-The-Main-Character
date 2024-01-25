using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantSoundController : SoundController
{
    public void FootStep()
    {
        Play(0);
    }

    public void roar()
    {
        Play(1);
    }

    public void punch()
    {
        Play(2);
    }

    public void breath()
    {
        Play(3);
    }
}
