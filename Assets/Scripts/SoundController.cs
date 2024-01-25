using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    
        [SerializeField] List<AudioSource> Sources = new List<AudioSource>();

        public void Play(int clipnumber)
        {
            Sources[clipnumber].Play();
        }
 

}
