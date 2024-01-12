using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSwordController : MonoBehaviour
{
    [SerializeField] Transform HeroPosition;
    [SerializeField] float KnockbackForce = 40.0f;
    [SerializeField] float KnockbackUpForce = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        Rigidbody EnemyRigidbody = other.GetComponent<Rigidbody>();
        Vector3 direction = HeroPosition.position - other.transform.position;
        direction.y = 0.0f;
        direction = -direction.normalized;
        direction.y = KnockbackUpForce;
        EnemyRigidbody.AddForce(direction * KnockbackForce);
        RegularEnemy enemy = other.GetComponent<RegularEnemy>();
        enemy.EnemyStruck();
    }
}
