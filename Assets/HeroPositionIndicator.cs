using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPositionIndicator : MonoBehaviour
{
    private GameObject player, hero;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        hero = GameObject.Find("HeroCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = player.transform.position;
        position.y = .15f;
        transform.position = position;
        Vector3 lookDirection = hero.transform.position - player.transform.position;
        lookDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
