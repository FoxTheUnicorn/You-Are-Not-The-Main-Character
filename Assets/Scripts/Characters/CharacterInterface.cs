using System.Collections;
using System.Collections.Generic;

public interface Character
{
    public List<Character> getEnemyList();
    public void hitEnemy(Character enemy);
    public void receiveHit(Character attacker);
    public bool isInvisible();
    public void setAnimationProperty(string property, bool isSet);
}
