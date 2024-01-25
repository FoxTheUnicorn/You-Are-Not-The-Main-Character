using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    private Animator HeroAnimator;
    [SerializeField] HitBox SlashHitbox;

    private void Start()
    {
        HeroAnimator = GetComponent<Animator>();
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

        foreach (Collider collider in colliders)
        {
            EnemyCharacter otherCharacter;
            if ((otherCharacter = (collider.GetComponent("EnemyCharacter") as EnemyCharacter)) != null && otherCharacter.getCanHit())
            {
                Debug.Log("Hit " + collider.gameObject.name);
            }
        }
    }
}
