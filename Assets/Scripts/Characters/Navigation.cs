using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Navigation : MonoBehaviour
{
    private const int TARGETSETTINGINACTIVE = 0;
    private const int SETTINGTARGETWITHINRECTANGLE = 1;
    private const int SETTINGTARGETWITHINELLIPSE = 2;
    private const int TARGETSETTINGPENDING = 3;
    private const int ENEMYTARGET = 4;

    private const int WANDERINGAROUNDAIMLESSLYINACTIVE = 0;
    private const int WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE = 1;
    private const int WANDERINGAROUNDAIMLESSLYWITHINELLIPSE = 2;

    public NavMeshAgent navMeshAgent;

    private float randomX1, randomZ1, randomX2, randomZ2;
    private int statusTargetSetting = TARGETSETTINGINACTIVE, statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
    private float restingTimeMin = 1.5f, restingTimeMax = 6f;
    private List<Character> enemyList = new List<Character>();
    private protected Character ownCharacter = null;

    private const float initialAttackRadius = 20f;
    private float attackRadius = initialAttackRadius, attackRadiusInc = .001f, hitRadius = 2.2f;
    private int checkIfShouldAttackCounter;

    private Character attackedCharacter = null;

    private float time = 0f, enemySightedTime = 0f;

    bool canHit;

    private float initialNavMeshSpeed, navMeshSpeedRunMultiplier = 2f;

    private float slowDownFactor = 1f;

    public void characterKilled(Character character)
    {
        if (character == attackedCharacter)
        {
            attackedCharacter = null;
            if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                statusTargetSetting = SETTINGTARGETWITHINRECTANGLE;
            else if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE)
                statusTargetSetting = SETTINGTARGETWITHINELLIPSE;
            else
                statusTargetSetting = TARGETSETTINGINACTIVE;
            enemySightedTime = 0f;
            attackRadius = initialAttackRadius;
            canHit = false;
            ownCharacter.setAnimationPropertyBool("RandomWalk", false);
            ownCharacter.setAnimationPropertyBool("RandomRoar", false);
            ownCharacter.setAnimationPropertyBool("FoundEnemy", false);
            navMeshAgent.speed = initialNavMeshSpeed * slowDownFactor;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        initialNavMeshSpeed = navMeshAgent.speed;
        navMeshAgent.isStopped = true;
        ownCharacter.setAnimationPropertyBool("RandomWalk", false);
        checkIfShouldAttackCounter = (int)Random.Range(0f, 10f);
        //floorBounds = boden.GetComponent<Renderer>().bounds;

        //Testschrott:
        startWanderingAroundAimlesslyWithinRectangle(-60f, -43f, 60f, 43f);
    }

    public void setOwnCharacterReference(Character ownCharacterReference)
    {
        ownCharacter = ownCharacterReference;
    }

    public void setRestingTime(float timeMin, float timeMax)
    {
        restingTimeMin = timeMin;
        restingTimeMax = timeMax;
    }

    private void setRandomTargetWithinRectangleInternal()
    {
        Vector3 targetLocation = new Vector3(randomX1 + Random.Range(0f, 1f) * (randomX2 - randomX1), 0f, randomZ1 + Random.Range(0f, 1f) * (randomZ2 - randomZ1));
        navMeshAgent.SetDestination(targetLocation);
        if (targetLocation.x < navMeshAgent.destination.x - 0.001f || targetLocation.x > navMeshAgent.destination.x + 0.001f || targetLocation.z < navMeshAgent.destination.z - 0.001f || targetLocation.z > navMeshAgent.destination.z + 0.001f)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            statusTargetSetting = SETTINGTARGETWITHINRECTANGLE;
        }
        else
        {
            statusTargetSetting = TARGETSETTINGINACTIVE;
            //ziel.transform.position = new Vector3(navMeshAgent.destination.x, 1f, navMeshAgent.destination.z);
            navMeshAgent.isStopped = false;
        }
    }

    private void setRandomTargetWithinEllipseInternal()
    {
        float centerX = randomX1 + (randomX2 - randomX1) / 2f;
        float centerZ = randomZ1 + (randomZ2 - randomZ1) / 2f;
        float radiusX = centerX - randomX1;
        float radiusZ = centerZ - randomZ1;
        float radiusZCorrection = radiusX / radiusZ;
        Vector3 targetLocation;
        do
        {
            targetLocation = new Vector3(randomX1 + Random.Range(0f, 1f) * (randomX2 - randomX1), 0f, randomZ1 + Random.Range(0f, 1f) * (randomZ2 - randomZ1));
        }
        while (((targetLocation.x - centerX) * (targetLocation.x - centerX) + (targetLocation.z - centerZ) * (targetLocation.z - centerZ) * radiusZCorrection * radiusZCorrection) > radiusX * radiusX);
        navMeshAgent.SetDestination(targetLocation);
        if (targetLocation.x < navMeshAgent.destination.x - 0.001f || targetLocation.x > navMeshAgent.destination.x + 0.001f || targetLocation.z < navMeshAgent.destination.z - 0.001f || targetLocation.z > navMeshAgent.destination.z + 0.001f)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            statusTargetSetting = SETTINGTARGETWITHINELLIPSE;
        }
        else
        {
            statusTargetSetting = TARGETSETTINGINACTIVE;
            navMeshAgent.isStopped = false;
        }
    }

    public void setRandomTargetWithinRectangle(float x1, float z1, float x2, float z2)
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        randomX1 = x1;
        randomZ1 = z1;
        randomX2 = x2;
        randomZ2 = z2;
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
        setRandomTargetWithinRectangleInternal();
    }

    public void setRandomTargetWithinEllipse(float centerX, float centerZ, float radiusX, float radiusZ)
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        randomX1 = centerX - radiusX;
        randomZ1 = centerZ - radiusZ;
        randomX2 = centerX + radiusX;
        randomZ2 = centerZ + radiusZ;
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
        setRandomTargetWithinEllipseInternal();
    }

    public void stopWalking()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
        statusTargetSetting = TARGETSETTINGINACTIVE;
        attackedCharacter = null;
        enemySightedTime = 0f;
        attackRadius = initialAttackRadius;
        canHit = false;
        ownCharacter.setAnimationPropertyBool("RandomWalk", false);
        ownCharacter.setAnimationPropertyBool("RandomRoar", false);
        ownCharacter.setAnimationPropertyBool("FoundEnemy", false);
        ownCharacter.setAnimationPropertyBool("EnemyInRange", false);
    }

    public void disableAttack()
    {
        attackRadius = 0f;
        attackRadiusInc = 0f;
    }

    public void startWanderingAroundAimlesslyWithinRectangle(float x1, float z1, float x2, float z2)
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        randomX1 = x1;
        randomZ1 = z1;
        randomX2 = x2;
        randomZ2 = z2;
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE;
        setRandomTargetWithinRectangleInternal();
    }

    public void startWanderingAroundAimlesslyWithinEllipse(float centerX, float centerZ, float radiusX, float radiusZ)

    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        randomX1 = centerX - radiusX;
        randomZ1 = centerZ - radiusZ;
        randomX2 = centerX + radiusX;
        randomZ2 = centerZ + radiusZ;
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYWITHINELLIPSE;
        setRandomTargetWithinEllipseInternal();
    }

    public virtual void setEnemyList(List<Character> newList)
    {
        enemyList = newList;
    }

    public void checkIfShouldAttack()
    {
        float smallestRadius = attackRadius;
        Vector3 position = new Vector3(0f, 0f, 0f);
        bool didFollowPlayer = false;
        if (attackedCharacter is PlayerCharacter)
        {
            attackedCharacter = null;

            didFollowPlayer = true;
        }
        foreach (Character character in enemyList)
        {
            float radius = (character.getPosition() - transform.position).magnitude;
            if (radius < smallestRadius && (!(character is PlayerCharacter) || !((PlayerCharacter)character).getIsStealth()))
            {
                smallestRadius = radius;
                attackedCharacter = character;
            }
        }
        if (attackedCharacter == null && ownCharacter is HeroCharacter)
        {
            GameObject player = GameObject.Find("Player");
            float abstand = (player.transform.position - transform.position).magnitude;
            if (abstand < initialAttackRadius * 1.2f && abstand > initialAttackRadius * .5f)
            {
                attackedCharacter = player.GetComponent<PlayerCharacter>();
            }
            else if (didFollowPlayer)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
                ownCharacter.setAnimationPropertyBool("EnemyInRange", false);
                ownCharacter.setAnimationPropertyBool("FoundEnemy", false);
                if (abstand >= initialAttackRadius * 1.2f)
                {
                    if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                        statusTargetSetting = SETTINGTARGETWITHINRECTANGLE;
                    else if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE)
                        statusTargetSetting = SETTINGTARGETWITHINELLIPSE;
                    else
                        statusTargetSetting = TARGETSETTINGINACTIVE;

                }
            }
            //PlayerCharacter playerScript = player.GetComponent<PlayerCharacter>();

        }
        else if (attackedCharacter == null && didFollowPlayer)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            ownCharacter.setAnimationPropertyBool("EnemyInRange", false);
            ownCharacter.setAnimationPropertyBool("FoundEnemy", false);
            ownCharacter.setAnimationPropertyBool("RandomRoar", false);
            ownCharacter.setAnimationPropertyBool("RandomWalk", false);
            enemySightedTime = 0f;

            /*if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                statusTargetSetting = SETTINGTARGETWITHINRECTANGLE;
            else if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE)
                statusTargetSetting = SETTINGTARGETWITHINELLIPSE;
            else
                statusTargetSetting = TARGETSETTINGINACTIVE;*/

            statusTargetSetting = TARGETSETTINGPENDING;
            float restingTime = restingTimeMin + Random.Range(0f, .5f) * (restingTimeMax - restingTimeMin);
            if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                Invoke("setRandomTargetWithinRectangleInternal", restingTime);
            else
                Invoke("setRandomTargetWithinEllipseInternal", restingTime);

        }
        if (attackedCharacter != null)
        {
            statusTargetSetting = ENEMYTARGET;
            if (ownCharacter is EnemyCharacter)
            {
                ownCharacter.setAnimationPropertyBool("RandomRoar", true);
                if (enemySightedTime == 0f)
                    enemySightedTime = time;
                if (time - enemySightedTime < 2.6f)
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.ResetPath();
                }
                else if (time - enemySightedTime < 2.8f)
                {
                    ownCharacter.setAnimationPropertyBool("FoundEnemy", true);
                }
                else
                {
                    navMeshAgent.SetDestination(attackedCharacter.getPosition());
                    navMeshAgent.speed = initialNavMeshSpeed * navMeshSpeedRunMultiplier * slowDownFactor;
                }
            }
            else
            {
                navMeshAgent.SetDestination(attackedCharacter.getPosition());
                ownCharacter.setAnimationPropertyBool("FoundEnemy", true);
                navMeshAgent.speed = initialNavMeshSpeed * navMeshSpeedRunMultiplier * slowDownFactor;
            }
        }
    }

    public virtual void Update()
    {
        time += Time.deltaTime;
        attackRadius += attackRadiusInc;
        checkIfShouldAttackCounter++;
        if ((attackedCharacter != null) && (attackedCharacter.getPosition() - transform.position).magnitude <= hitRadius && (!(ownCharacter is HeroCharacter) || !(attackedCharacter is PlayerCharacter)))
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Vector3 lookDirection = attackedCharacter.getPosition() - transform.position;
            lookDirection.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 10f);
            ownCharacter.setAnimationPropertyBool("EnemyInRange", true);
            Invoke("enableHit", 0.2f);

            if ((attackedCharacter is PlayerCharacter) && (checkIfShouldAttackCounter >= 10))
            {
                checkIfShouldAttackCounter = 0;
                checkIfShouldAttack();
            }
        }
        else
        {
            ownCharacter.setAnimationPropertyBool("EnemyInRange", false);
            canHit = false;
            if (attackedCharacter != null)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackedCharacter.getPosition() - transform.position), Time.deltaTime * 1f);
            if (checkIfShouldAttackCounter >= 10)
            {
                checkIfShouldAttackCounter = 0;
                checkIfShouldAttack();
            }
        }

        if (statusTargetSetting != ENEMYTARGET && navMeshAgent.hasPath && (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE || statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE)) navMeshAgent.isStopped = false;
        ownCharacter.setAnimationPropertyBool("RandomWalk", navMeshAgent.hasPath /*&& !navMeshAgent.isStopped*/);
        ownCharacter.setAnimationPropertyFloat("WalkingSpeed", navMeshAgent.velocity.magnitude / (navMeshAgent.speed * slowDownFactor));

        if (statusTargetSetting == SETTINGTARGETWITHINRECTANGLE)
            setRandomTargetWithinRectangleInternal();
        else if (statusTargetSetting == SETTINGTARGETWITHINELLIPSE)
            setRandomTargetWithinEllipseInternal();
        else if (!navMeshAgent.hasPath && !navMeshAgent.pathPending && (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE || statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE) && statusTargetSetting != TARGETSETTINGPENDING && statusTargetSetting != ENEMYTARGET)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            statusTargetSetting = TARGETSETTINGPENDING;
            float restingTime = restingTimeMin + Random.Range(0f, 1f) * (restingTimeMax - restingTimeMin);
            if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                Invoke("setRandomTargetWithinRectangleInternal", restingTime);
            else
                Invoke("setRandomTargetWithinEllipseInternal", restingTime);
        }
    }

    void enableHit()
    {
        if (ownCharacter.getAnimationPropertyBool("EnemyInRange"))
            canHit = true;
    }

    public bool getCanHit()
    {
        return canHit;
    }

    public virtual void FixedUpdate()
    {
    }

    public void slowDown()
    {
        float slowDownInc = 0.987f;
        slowDownFactor *= slowDownInc;
        if (slowDownFactor < 0.5f) slowDownFactor = 0.5f;
        navMeshAgent.speed = initialNavMeshSpeed * slowDownFactor;
        if (attackedCharacter != null) navMeshAgent.speed *= navMeshSpeedRunMultiplier;
    }

    public float getSlowDownFactor()
    {
        return slowDownFactor;
    }
}
