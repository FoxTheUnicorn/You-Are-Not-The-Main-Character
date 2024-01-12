using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, Character
{
    private List<Character> enemyList = new List<Character>();
    CharacterManager characterManager;

    // Start is called before the first frame update
    void Start()
    {
        registerCharacterManager();
        characterManager.registerCharacter(this);
        setEnemyList(characterManager.getEnemyCharacterList(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    private void registerCharacterManager()
    {
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
    }

    private void setEnemyList(List<Character> newList)
    {
        enemyList = newList;
    }

    public List<Character> getEnemyList()
    {
        return enemyList;
    }

    public void hitEnemy(Character enemy) { }

    public void receiveHit(Character attacker) { }
}
