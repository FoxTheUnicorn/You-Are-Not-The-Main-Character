using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularEnemy : MonoBehaviour {
    private ParticleSystem DeathParticles;
    private Rigidbody EnemyRigidbody;

     void Start()
    {
        EnemyRigidbody = this.GetComponent<Rigidbody>();
        DeathParticles = this.GetComponent<ParticleSystem>();
    }
    public void EnemyStruck()
    {
        DeathParticles.Play();
        EnemyRigidbody.freezeRotation = false;
    }
}
