using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private List<Character> goodCharacterList = new List<Character>(), badCharacterList = new List<Character>();

    public void registerCharacter(Character newCharacter)
    {
        if (newCharacter is EnemyCharacter)
            badCharacterList.Add(newCharacter);
        else
            goodCharacterList.Add(newCharacter);
    }

    public void removeCharacter(Character character)
    {
        goodCharacterList.Remove(character);
        badCharacterList.Remove(character);
    }

    public List<Character> getGoodCharacterList()
    {
        return goodCharacterList;
    }

    public List<Character> getBadCharacterList()
    {
        return badCharacterList;
    }

    public int GetBadCharacterCount()
    {
        return badCharacterList.Count;
    }

    public List<Character> getEnemyCharacterList(Character ownCharacter)
    {
        if (ownCharacter is EnemyCharacter)
            return goodCharacterList;
        else
            return badCharacterList;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
