using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    private Animator HeroAnimator;
    [SerializeField] HitBox SlashHitbox;
    [SerializeField] private HeroCharacter ownCharacter;

    public void SlashAttack()
    {
        HashSet<Collider> colliders = SlashHitbox.Hit();
        
        foreach (Collider collider in colliders)
        {
            EnemyCharacter otherCharacter;
            if ((otherCharacter = (collider.GetComponent("EnemyCharacter") as EnemyCharacter)) != null && ownCharacter.getCanHit())
            {
                //Debug.Log("Hit " + collider.gameObject.name);
                ownCharacter.hitEnemy(otherCharacter);
            }
        }
    }
}
