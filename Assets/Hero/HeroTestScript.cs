using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTestScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        TogglePresses("z", "EnemyInRange");
        TogglePresses("x", "EnemySighted");
        TogglePresses("c", "RandomWalk");
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
