using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCCharacter : Navigation, Character
{
    private List<Character> enemyList = new List<Character>();
    CharacterManager characterManager;
    public Animator animator;

    private int minDamage = 20, maxDamage = 40;
    private protected int maxHealth = 100, health;
    public RegularEnemy regularEnemy;

    // Start is called before the first frame update
    public override void Start()
    {
        health = maxHealth;
        setOwnCharacterReference(this);
        registerCharacterManager();
        characterManager.registerCharacter(this);
        setEnemyList(characterManager.getEnemyCharacterList(this));
        base.Start();
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void registerCharacterManager()
    {
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
    }

    private void setEnemyList(List<Character> newList)
    {
        enemyList = newList;
        base.setEnemyList(newList);
    }

    public List<Character> getEnemyList()
    {
        return enemyList;
    }

    public virtual bool isInvisible()
    {
        return false;
    }

    public void setAnimationPropertyBool(string property, bool isSet)
    {
        if (animator.GetBool(property) != isSet)
        {
            animator.SetBool(property, isSet);
        }
    }

    public void setAnimationPropertyFloat(string property, float value)
    {
        animator.SetFloat(property, value);
    }

    public bool getAnimationPropertyBool(string property)
    {
        return animator.GetBool(property);
    }

    public void hitEnemy(Character enemy)
    {
        enemy.receiveHit(ownCharacter, (int)(minDamage + Random.Range(0, maxDamage - minDamage + 1)));
    }

    public bool receiveHit(Character enemy, int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            stopWalking();
            regularEnemy.EnemyStruck();
            characterManager.removeCharacter(ownCharacter);
            disableAttack();
            //List<Character> enemyList = ownCharacter.getEnemyList();
            foreach (Character character in enemyList)
            {
                if (character is NPCCharacter)
                {
                    NPCCharacter npcCharacter = (NPCCharacter)character;
                    npcCharacter.characterKilled(ownCharacter);
                }
            }
            return true;
        }
        return false;
    }
}
