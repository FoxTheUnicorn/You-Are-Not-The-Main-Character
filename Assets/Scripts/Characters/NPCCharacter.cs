using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCCharacter : Navigation, Character
{
    private List<Character> enemyList = new List<Character>();
    CharacterManager characterManager;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        base.setOwnCharacterReference(this);
        registerCharacterManager();
        characterManager.registerCharacter(this);
        enemyList = characterManager.getEnemyCharacterList(this);
        setEnemyList(enemyList);
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

}
