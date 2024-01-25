using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private HashSet<Collider> colliders = new HashSet<Collider>();
    public Animator animator;

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject is EnemyCharacter)
        EnemyCharacter otherCharacter;
        if ((otherCharacter = (other.GetComponent("EnemyCharacter") as EnemyCharacter)) != null && otherCharacter.getCanHit())
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 0.3f && info.normalizedTime < 0.6)
            {
                colliders.Add(other);
                Debug.Log("Addded " + other.gameObject.name);
            }
        }
    }    
    void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        Debug.Log("Removed " + other.gameObject.name);
    }

    private void OnEnable()
    {
        colliders = new HashSet<Collider>();    //This could be really unperformant
    }

    public HashSet<Collider> Hit()
    {
        return colliders;
    }
}
