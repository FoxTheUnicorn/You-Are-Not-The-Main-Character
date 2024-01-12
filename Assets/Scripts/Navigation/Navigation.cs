using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private const int TARGETSETTINGINACTIVE = 0;
    private const int SETTINGTARGETWITHINRECTANGLE = 1;
    private const int SETTINGTARGETWITHINELLIPSE = 2;
    private const int TARGETSETTINGPENDING = 3;

    private const int WANDERINGAROUNDAIMLESSLYINACTIVE = 0;
    private const int WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE = 1;
    private const int WANDERINGAROUNDAIMLESSLYWITHINELLIPSE = 2;

    public NavMeshAgent navMeshAgent;
    public GameObject boden;

    private Bounds floorBounds;
    private float randomX1, randomZ1, randomX2, randomZ2;
    private int statusTargetSetting = TARGETSETTINGINACTIVE, statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
    private float restingTimeMin = 0.5f, restingTimeMax = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        floorBounds = boden.GetComponent<Renderer>().bounds;

        //Testschrott:
        startWanderingAroundAimlesslyWithinEllipse(-145.4f, 0f, 10f, 15f);
    }

    void setWalkingSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    float getWalkingSpeed()
    {
        return navMeshAgent.speed;
    }

    void setRestingTime(float timeMin, float timeMax)
    {
        restingTimeMin = timeMin;
        restingTimeMax = timeMax;
    }

    private void setRandomTargetWithinRectangleInternal()
    {
        Vector3 targetLocation = new Vector3(randomX1 + Random.Range(0f, 1f) *(randomX2 - randomX1), 0f, randomZ1 + Random.Range(0f, 1f) *(randomZ2 - randomZ1));
        navMeshAgent.SetDestination(targetLocation);
        if (targetLocation.x < navMeshAgent.destination.x - 0.001f || targetLocation.x > navMeshAgent.destination.x + 0.001f || targetLocation.z < navMeshAgent.destination.z - 0.001f || targetLocation.z > navMeshAgent.destination.z + 0.001f)
        {
            navMeshAgent.isStopped = true;
            statusTargetSetting = SETTINGTARGETWITHINRECTANGLE;
        }
        else
        {
            statusTargetSetting = TARGETSETTINGINACTIVE;
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
        do {
            targetLocation = new Vector3(randomX1 + Random.Range(0f, 1f) * (randomX2 - randomX1), 0f, randomZ1 + Random.Range(0f, 1f) * (randomZ2 - randomZ1));
        }
        while (((targetLocation.x - centerX) * (targetLocation.x - centerX) + (targetLocation.z - centerZ) * (targetLocation.z - centerZ) * radiusZCorrection * radiusZCorrection) > radiusX * radiusX);
        navMeshAgent.SetDestination(targetLocation);
        if (targetLocation.x < navMeshAgent.destination.x - 0.001f || targetLocation.x > navMeshAgent.destination.x + 0.001f || targetLocation.z < navMeshAgent.destination.z - 0.001f || targetLocation.z > navMeshAgent.destination.z + 0.001f)
        {
            navMeshAgent.isStopped = true;
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
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYINACTIVE;
        statusTargetSetting = TARGETSETTINGINACTIVE;
    }

    public void startWanderingAroundAimlesslyWithinRectangle(float x1, float z1, float x2, float z2)
    {
        navMeshAgent.isStopped = true;
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
        randomX1 = centerX - radiusX;
        randomZ1 = centerZ - radiusZ;
        randomX2 = centerX + radiusX;
        randomZ2 = centerZ + radiusZ;
        statusWanderingAroundAimlessly = WANDERINGAROUNDAIMLESSLYWITHINELLIPSE;
        setRandomTargetWithinEllipseInternal();
    }

    void Update()
    {
        if (statusTargetSetting == SETTINGTARGETWITHINRECTANGLE)
            setRandomTargetWithinRectangleInternal();
        else if (statusTargetSetting == SETTINGTARGETWITHINELLIPSE)
            setRandomTargetWithinEllipseInternal();
        else if (!navMeshAgent.hasPath && (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE || statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINELLIPSE) && statusTargetSetting != TARGETSETTINGPENDING)
        {
            statusTargetSetting = TARGETSETTINGPENDING;
            float restingTime = restingTimeMin + Random.Range(0f, 1f) * (restingTimeMax - restingTimeMin);
            if (statusWanderingAroundAimlessly == WANDERINGAROUNDAIMLESSLYWITHINRECTANGLE)
                Invoke("setRandomTargetWithinRectangleInternal", restingTime);
            else
                Invoke("setRandomTargetWithinEllipseInternal", 10f);
        }
    }
}
