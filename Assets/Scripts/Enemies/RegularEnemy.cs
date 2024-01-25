using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularEnemy : MonoBehaviour {
    [SerializeField] float DespawnTime = 5.0f;
    public GameObject ownCharacter;
    public ParticleSystem DeathParticles;
    private Rigidbody EnemyRigidbody;
    private Vector3 torque;
    bool killed = false;

     void Start()
    {
        EnemyRigidbody = this.GetComponent<Rigidbody>();
        //DeathParticles = this.GetComponent<ParticleSystem>();
    }
    public void EnemyStruck()
    {
        DeathParticles.Play();
        //EnemyRigidbody.freezeRotation = false;
        //EnemyRigidbody.freezePosition = false;
        EnemyRigidbody.constraints = RigidbodyConstraints.None;
        EnemyRigidbody.AddTorque(RandomTorque());
        Invoke("CommenceDeath", DespawnTime);
        DeathParticles.Play();
    }

    private Vector3 RandomTorque()
    {
        int vorzeichen = (int)(Random.Range(0f, 2f)) * 2 - 1;
        torque.x = vorzeichen * Random.Range(500, 800);
        vorzeichen = (int)(Random.Range(0f, 2f)) * 2 - 1;
        torque.y = vorzeichen * Random.Range(500, 800);
        vorzeichen = (int)(Random.Range(0f, 2f)) * 2 - 1;
        torque.z = vorzeichen * Random.Range(500, 800);
        return torque;
    }
    public void CommenceDeath()
    {
        DeathParticles.Stop();
        DeathParticles.Play();
        Invoke("RemoveCharacter", 2f);
        killed = true;
    }

    private void RemoveCharacter()
    {
        Destroy(ownCharacter);
    }

    public void Update()
    {
        if (killed)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 2f);
    }
}
