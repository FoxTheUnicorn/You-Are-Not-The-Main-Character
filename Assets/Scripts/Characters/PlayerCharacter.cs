using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, Character
{
    private List<Character> enemyList = new List<Character>();
    CharacterManager characterManager;

    [SerializeField] private float playerSpeed = 1.0f;  //Speed in m/s
    [SerializeField] private float sprintSpeed = 10.0f; //Speed in m/s
    [SerializeField] private float sprintDuration = 5.0f; //Sprint duration in seconds
    [SerializeField] private float sprintRegenerationDelay = 1.0f; //Delay before Sprint regenerates
    [SerializeField] private float sprintRegenerationSpeed = 0.5f; //Sprint regained per second
    [SerializeField] private bool applyGravity = true;

    [SerializeField] private float gravity = 0.2f;
    [SerializeField] private float terminalVelocity = 2.0f;
    [SerializeField] private float stamina;           //How much sprint the Player has left
    [SerializeField] private float sprintCooldown;   //How long before sprint starts regenerating

    [SerializeField] private float vSpeed = 0.0f;
    private Vector3 inputDirection;

    private CharacterController controller;

    public bool PlayerIsMoving()
    {
        return inputDirection.magnitude > 0.1f;
    }

    private void MovePlayer()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isSprinting = Input.GetButton("Sprint");
        if (controller.isGrounded)
        {
            vSpeed = 0;
        }

        if (!PlayerIsMoving())
        {
            regenerateSprint();
        }

        if (inputDirection.magnitude > 1.0f) inputDirection = inputDirection / inputDirection.magnitude;    //Nerf diagonal movement

        Vector3 movement = transform.TransformDirection(inputDirection) * Time.deltaTime;
        if (applyGravity)
        {
            if (controller.isGrounded) vSpeed = 0;
            else
            {
                vSpeed -= gravity * Time.deltaTime;
                if (vSpeed < -terminalVelocity)
                {
                    vSpeed = -terminalVelocity;
                }
            }
            movement.y = vSpeed;
        }


        if (!isSprinting)                               //If Player is not holding Sprint
        {
            regenerateSprint();
            controller.Move(movement * playerSpeed);
            return;
        }

        if (stamina > 0.0f)                              //If Player is holding Sprint but doesnt have stamina
        {
            sprintCooldown = sprintRegenerationDelay;
            stamina -= Time.deltaTime;
            controller.Move(movement * sprintSpeed);
        }
        else                                            //If Player has no Sprint left
        {
            controller.Move(movement * playerSpeed);
        }
    }

    private void regenerateSprint()
    {
        if (stamina == sprintDuration) return;                   //Not used any sprint

        if (sprintCooldown > 0.0f)                             //If sprint regeneration is still on cooldown
        {
            sprintCooldown -= Time.deltaTime;
            return;
        }

        if (stamina < 0.0f) stamina = 0.0f;                       //If over exhausted

        stamina += sprintRegenerationSpeed * Time.deltaTime;

        if (stamina > sprintDuration) stamina = sprintDuration;   //If over regenerated
    }

    public void regainSprint()
    {
        stamina = sprintDuration;
    }

    public void setSprintDuration(float duration)
    {
        sprintDuration = duration;
    }

    // Start is called before the first frame update
    void Start()
    {
        registerCharacterManager();
        characterManager.registerCharacter(this);
        setEnemyList(characterManager.getEnemyCharacterList(this));

        stamina = sprintDuration;
        sprintCooldown = sprintRegenerationDelay;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void registerCharacterManager()
    {
        characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
    }

    private void setEnemyList(List<Character> newList)
    {
        enemyList = newList;
    }

    public List<Character> getEnemyList()
    {
        return enemyList;
    }

    public void hitEnemy(Character enemy) { }

    public void receiveHit(Character attacker) { }
}
