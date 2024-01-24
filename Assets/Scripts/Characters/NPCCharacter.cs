using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCCharacter : Navigation, Character
{
    private List<Character> enemyList = new List<Character>();
    CharacterManager characterManager;
    public Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
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

    public virtual void hitEnemy(Character enemy) { }

    public virtual void receiveHit(Character attacker) { }

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
}
