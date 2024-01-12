using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularEnemy : MonoBehaviour {
    [SerializeField] float DespawnTime = 5.0f;
    private ParticleSystem DeathParticles;
    private Rigidbody EnemyRigidbody;
    private Vector3 torque;

     void Start()
    {
        EnemyRigidbody = this.GetComponent<Rigidbody>();
        DeathParticles = this.GetComponent<ParticleSystem>();
    }
    public void EnemyStruck()
    {
        DeathParticles.Play();
        EnemyRigidbody.freezeRotation = false;
        EnemyRigidbody.AddTorque(RandomTorque());
        Invoke("CommenceDeath", DespawnTime);
    }

    private Vector3 RandomTorque()
    {
        torque.x = Random.Range(-200, 200);
        torque.y = Random.Range(-200, 200);
        torque.z = Random.Range(-200, 200);
        return torque;
    }
    public void CommenceDeath()
    {
        DeathParticles.Play();
        Destroy(gameObject, DeathParticles.main.duration);
        transform.GetChild(0).localScale = new Vector3(0, 0, 0);
    }
}
