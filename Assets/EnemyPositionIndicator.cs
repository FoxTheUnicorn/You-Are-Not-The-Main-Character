using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionIndicator : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = player.transform.position;
        position.y = .15f;
        transform.position = position;

        Vector3 closestEnemyPosition = player.GetComponent<PlayerCharacter>().getClosestEnemyPosition();

        if (closestEnemyPosition.magnitude < 100000f)
        {
            Vector3 lookDirection = closestEnemyPosition - player.transform.position;
            lookDirection.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
        }
    }
}