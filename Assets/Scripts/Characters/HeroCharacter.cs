using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : NPCCharacter
{
    public int startHealth = 1000;
    private float delayedHealth = 0f;
    private UIHealthController uiHealthController;
    // Start is called before the first frame update
    public override void Start()
    {
        maxHealth = startHealth;
        uiHealthController = GameObject.Find("HeroHPBar").GetComponent<UIHealthController>();
        uiHealthController.InitHealthBar(maxHealth);
        base.Start();
        minDamage = 40;
        maxDamage = 60;
    }

    // Update is called once per frame
    public override void Update()
    {
        uiHealthController.UpdateHealthBar(delayedHealth);
        if (delayedHealth > health)
        {
            delayedHealth -= (maxHealth / 2f) * Time.deltaTime;
            if (delayedHealth < health) delayedHealth = health;
        }
        else if (delayedHealth < health)
        {
            delayedHealth += (maxHealth / 2f) * Time.deltaTime;
            if (delayedHealth > health) delayedHealth = health;
        }
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.Update();
    }
}
