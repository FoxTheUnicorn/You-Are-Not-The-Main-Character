using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRune : MonoBehaviour
{
    [SerializeField] ParticleSystem UnarmedFreeze;
    [SerializeField] ParticleSystem UnArmedToArmed;
    [SerializeField] ParticleSystem ArmedFreeze;

    [SerializeField] ParticleSystem ActiveFreeze; 
    
    [SerializeField] float movementSpeedFactor = 0.25f;
    [SerializeField] float ArmingTime = 3.0f;
    [SerializeField] bool armed = false;
    // Start is called before the first frame update    
    bool triggered = false;

    private void Start()
    {
        UnarmedFreeze.Play();
        Invoke("ArmRuneAnimation", ArmingTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!armed) return;
        if (triggered) return;
        //if (!other.CompareTag("Enemy")) return;
        ActiveFreeze.Play();
        ArmedFreeze.Stop();
        ArmedFreeze.gameObject.SetActive(false);
        triggered = true;
        Invoke("DisableEffect", ActiveFreeze.main.duration);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!triggered) return;
        //TODO Jörn
        /* EnemyScript script = other.GetComponent<EnemyScript>();
        if (script == null) return;
        script.SetSpeed(script.GetSpeed * movementSpeedFactor) */
    }

    public void ArmRuneAnimation()
    {
        UnArmedToArmed.Play();
        UnarmedFreeze.gameObject.SetActive(false);
        Invoke("ArmRune", UnArmedToArmed.main.duration);
    }
    public void ArmRune()
    {
        ArmedFreeze.Play();
        UnArmedToArmed.gameObject.SetActive(false);
        armed = true;
    }

    public void DisableEffect()
    {
        triggered = false;
        ActiveFreeze.gameObject.SetActive(false);
        Invoke("KillRune", 1.0f);
    }

    public void KillRune()
    {
        Destroy(gameObject);
    }
}
