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

    [SerializeField] private Animator animator;

    private bool isStealth = false;

    private float vSpeed = 0.0f;
    private Vector3 inputDirection;
    private Vector3 inputVectors;

    private CharacterController controller;

    public Vector3 getPosition()
    {
        return transform.position;
    }

    private void MovePlayer()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isMoving = inputDirection.magnitude > 0.0f;
        bool isSprinting = Input.GetButton("Sprint");

        if (inputDirection.magnitude > 1.0f) inputDirection = inputDirection / inputDirection.magnitude;    //Nerf diagonal movement

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
            inputDirection.y = vSpeed;
        }

        Vector3 movement = transform.TransformDirection(inputDirection) * 0.02f;

        if (!isMoving)
        {
            animator.SetInteger("Speed", 0);
            regenerateSprint();
            return;
        }

        if (!isSprinting)                               //If Player is not holding Sprint
        {
            animator.SetInteger("Speed", 1);
            regenerateSprint();
            controller.Move(movement * playerSpeed);
            return;
        }

        if (stamina > 0.0f)                              //If Player is holding Sprint but doesnt have stamina
        {
            animator.SetInteger("Speed", 2);
            sprintCooldown = sprintRegenerationDelay;
            stamina -= Time.deltaTime;
            controller.Move(movement * sprintSpeed);
        }
        else                                            //If Player has no Sprint left
        {
            animator.SetInteger("Speed", 1);
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

    public void ActivateStealth(float duration)
    {
        isStealth = true;
        animator.SetBool("Stealth", true);
        Invoke("DeactivateStealth", duration);
    }

    public void DeactivateStealth()
    {
        isStealth = false;
        animator.SetBool("Stealth", false);
    }

    public void setSprintDuration(float duration)
    {
        sprintDuration = duration;
    }

    // Start is called before the first frame update
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        regainSprint();
        registerCharacterManager();
        characterManager.registerCharacter(this);
        setEnemyList(characterManager.getEnemyCharacterList(this));
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

    public virtual bool isInvisible()
    {
        return false;
    }

    public void setAnimationPropertyBool(string property, bool isSet)
    {
        if (animator.GetBool(property) != isSet)
            animator.SetBool(property, isSet);
    }


    public void setAnimationPropertyFloat(string property, float value)
    {
        animator.SetFloat(property, value);
    }
}
