using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    private Animator HeroAnimator;
    [SerializeField] HitBox SlashHitbox;

    public void SlashAttack()
    {
        HashSet<Collider> colliders = SlashHitbox.Hit();
        
        foreach(Collider collider in colliders)
        {
            //TODO Gegner Filtern + Treffen
        }
    }
}
