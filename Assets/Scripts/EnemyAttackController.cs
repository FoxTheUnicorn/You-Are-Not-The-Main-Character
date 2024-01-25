using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] HitBox PunchHitbox;

    public void PunchAttack()
    {
        HashSet<Collider> colliders = PunchHitbox.Hit();
        
        foreach(Collider collider in colliders)
        {
            //TODO Gegner Filtern + Treffen
        }
    }
}
