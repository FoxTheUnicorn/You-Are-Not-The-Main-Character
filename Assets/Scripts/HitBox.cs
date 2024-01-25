using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private HashSet<Collider> colliders = new HashSet<Collider>();
    public Animator animator;

    void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
        //Debug.Log("Added " + other.gameObject.name);
    }
    void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        //Debug.Log("Removed " + other.gameObject.name);
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