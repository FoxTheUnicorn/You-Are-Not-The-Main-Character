using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Character
{
    public List<Character> getEnemyList();
    public void hitEnemy(Character enemy);
    public void receiveHit(Character attacker);
    public bool isInvisible();
    public void setAnimationPropertyBool(string property, bool isSet);
    public void setAnimationPropertyFloat(string property, float value);
    public Vector3 getPosition();
}
