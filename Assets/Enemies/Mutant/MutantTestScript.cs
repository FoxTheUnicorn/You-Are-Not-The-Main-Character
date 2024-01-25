using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantTestScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        TogglePresses("z", "RandomWalk");
        TogglePresses("x", "FoundEnemy");
        TogglePresses("c", "RandomRoar");
        TogglePresses("v", "EnemyInRange");
    }

    void TogglePresses(string key, string boolarg)
    {
        if (Input.GetKey(key) && !animator.GetBool(boolarg))
        {
            animator.SetBool(boolarg, true);
        }
        else if (!Input.GetKey(key) && animator.GetBool(boolarg))
        {
            animator.SetBool(boolarg, false);
        }
    }
}
