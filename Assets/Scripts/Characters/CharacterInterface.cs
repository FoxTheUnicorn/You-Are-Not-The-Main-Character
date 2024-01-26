using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Character
{
    public List<Character> getEnemyList();
    public bool isInvisible();
    public void setAnimationPropertyBool(string property, bool isSet);
    public bool getAnimationPropertyBool(string property);
    public void setAnimationPropertyFloat(string property, float value);
    public Vector3 getPosition();
    public void hitEnemy(Character enemy);
    public bool receiveHit(Character enemy, int damage);
    public float getMaxHealth();
    public void heal(float amount);
}
