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

    [SerializeField] private float sprint;           //How much sprint the Player has left
    [SerializeField] private float sprintCooldown;   //How long before sprint starts regenerating

    private Vector3 inputDirection;

    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        sprint = sprintDuration;
        sprintCooldown = sprintRegenerationDelay;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isSprinting = Input.GetButton("Sprint");

        if (inputDirection.magnitude < 0.1f)
        {
            regenerateSprint();
            return;
        }

        if (inputDirection.magnitude > 1.0f) inputDirection = inputDirection / inputDirection.magnitude;    //Nerf diagonal movement

        Vector3 movement = transform.TransformDirection(inputDirection) * Time.deltaTime;

        if (!isSprinting)                               //If Player is not holding Sprint
        {
            regenerateSprint();
            controller.Move(movement * playerSpeed);
            return;
        }

        if (sprint > 0.0f)                              //If Player is holding Sprint an
        {
            sprintCooldown = sprintRegenerationDelay;
            sprint -= Time.deltaTime;
            controller.Move(movement * sprintSpeed);
        }
        else                                            //If Player has no Sprint left
        {
            controller.Move(movement * playerSpeed);
        }
    }

    private void regenerateSprint() {
        if (sprint == sprintDuration) return;                   //Not used any sprint

        if (sprintCooldown > 0.0f)                             //If sprint regeneration is still on cooldown
        {
            sprintCooldown -= Time.deltaTime;
            return;
        }

        if (sprint < 0.0f) sprint = 0.0f;                       //If over exhausted

        sprint += sprintRegenerationSpeed * Time.deltaTime;

        if (sprint > sprintDuration) sprint = sprintDuration;   //If over regenerated
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
