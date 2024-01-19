using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] List<InternalAbility> abilities = new List<InternalAbility>();
    [SerializeField] GameObject SpellCollection;

    public void Update()
    {
        foreach (InternalAbility ability in abilities)
        {
            ability.Update();
        }
        if(Input.GetKeyDown("1"))
        {
            CastAbility1();
        } else if(Input.GetKeyDown("2"))
        {
            CastAbility2();
        } else if(Input.GetKeyDown("3"))
        {
            CastAbility3();
        }
    }

    public void CastAbility1()
    {
        InternalAbility ability = abilities[0];
        if (!ability.IsOnCooldown())
        {
            ability.AbilityCooldown();
            //TODO Ability1 Effect
        }
    }      
    public void CastAbility2()
    {
        InternalAbility ability = abilities[1];
        if (!ability.IsOnCooldown())
        {
            ability.AbilityCooldown();
            Vector3 TargetPosition = transform.position;
            TargetPosition.y = 0;
            GameObject spell = Instantiate(ability.SpellObject, TargetPosition, Quaternion.identity, SpellCollection.transform);

        }
    }      
    public void CastAbility3()
    {
        InternalAbility ability = abilities[2];
        if (!ability.IsOnCooldown())
        {
            ability.AbilityCooldown();
            //TODO Ability1 Effect
        }
    }    




    [Serializable]
    public class InternalAbility 
    {
        public UIAbilityController UIAbility;
        public float Cooldown = 10.0f;
        private float CooldownTimer = -1.0f;   //-1.0f means inactive cooldown
        public GameObject SpellObject;

        public void AbilityCooldown()
        {
            CooldownTimer = Cooldown;
        }

        public bool IsOnCooldown()
        {
            return CooldownTimer > 0.0f;;
        }

        public void Update()
        {
            if (CooldownTimer == -1.0f) return;
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer < 0.0f) CooldownTimer = -1.0f;
            UIAbility.UpdateUI(CooldownTimer);
        }
    }
}
