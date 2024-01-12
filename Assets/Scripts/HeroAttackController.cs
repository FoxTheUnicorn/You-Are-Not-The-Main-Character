using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    [SerializeField] Animator HeroAnimator;
    [SerializeField] bool onCooldown = false;
    [SerializeField] float SpinCooldown = 3.0f;

    private float SpinTimer = 0.0f;

    public void StartSpin()
    {
        Invoke("ResetCooldown", SpinCooldown);
        HeroAnimator.SetTrigger("SpinTrigger");
        onCooldown = true;
    }

    public void ResetCooldown()
    {
        HeroAnimator.ResetTrigger("SpinTrigger");
        onCooldown = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (onCooldown) return;
        StartSpin();
    }
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (onCooldown) return;
        StartSpin();
    }
}
