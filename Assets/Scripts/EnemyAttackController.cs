using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private Animator EnemyAnimator;
    [SerializeField] HitBox SlashHitbox;

    private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

    }
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
    }

    public void SlashAttack()
    {
        HashSet<Collider> colliders = SlashHitbox.Hit();
        
        foreach(Collider collider in colliders)
        {
            //TODO Gegner Filtern + Treffen
        }
    }
}
