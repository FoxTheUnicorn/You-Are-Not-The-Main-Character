using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 1.0f;  //Speed in m/s
    [SerializeField] private float sprintSpeed = 10.0f; //Speed in m/s
    [SerializeField] private float sprintDuration = 5.0f; //Sprint duration in seconds
    [SerializeField] private float sprintRegenerationDelay = 1.0f; //Delay before Sprint regenerates
    [SerializeField] private float sprintRegenerationSpeed = 0.5f; //Sprint regained per second

    private float sprint;

    private Vector3 inputDirection;
    private 

    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        sprint = sprintDuration;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        if (inputDirection.magnitude == 0.0f) return;
        MovePlayer();
    }

    private void MovePlayer()
    {

    }

    public void regainSprint()
    {
        sprint = sprintDuration;
    }

    public void setSprintDuration(float duration)
    {
        sprintDuration = duration;
    }
}
