using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRune : MonoBehaviour
{
    [SerializeField] ParticleSystem ActiveFreeze;
    [SerializeField] ParticleSystem InActiveFreeze;
    [SerializeField] float movementSpeedFactor = 0.25f;
    // Start is called before the first frame update    
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        //if(!other.CompareTag("Enemy")) return;
        ActiveFreeze.Play();
        InActiveFreeze.Stop();
        InActiveFreeze.gameObject.SetActive(false);
        triggered = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //TODO
        /* EnemyScript script = other.GetComponent<EnemyScript>();
        if (script == null) return;
        script.SetSpeed(script.GetSpeed) */
    }
}
