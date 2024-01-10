using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegularEnemy : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private float HP = 20.0f;
    [SerializeField] private float MovementSpeed = 1.5f;
    [SerializeField] private float DPS = 1.0f;

    [SerializeField] UnityEvent OnDefeat;
    [SerializeField] UnityEvent OnSpawn;
    [SerializeField] UnityEvent OnWhistleHeard;
    [SerializeField] UnityEvent OnTrapTrigger;

    public void HearWhistle()
    {
        OnWhistleHeard.Invoke();
    }
}
