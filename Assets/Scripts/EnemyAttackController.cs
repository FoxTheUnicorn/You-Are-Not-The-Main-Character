using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] HitBox PunchHitbox;
    [SerializeField] private EnemyCharacter ownCharacter;

    public void PunchAttack()
    {
        HashSet<Collider> colliders = PunchHitbox.Hit();
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
            Character otherCharacter;
            if ((((otherCharacter = (collider.GetComponent("HeroCharacter") as Character)) != null) || ((otherCharacter = (collider.GetComponent("PlayerCharacter") as Character)) != null)) && ownCharacter.getCanHit())
            {
                Debug.Log("Hit " + collider.gameObject.name);
                ownCharacter.hitEnemy(otherCharacter);
            }
        }
    }
}
