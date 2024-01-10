using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyParentGameObject;  
    [SerializeField] private Transform spawnLocation;           //The actual Spawn Location of the Enemies
    [SerializeField] private Vector3 spread;                    //Random area around the spawnLocation where Enemies can spawn
    [SerializeField] private float minDistanceToPlayer = 4.0f;  //The minimum distance to the Player to Spawn an Enemy
    [SerializeField] private float spawnCooldown = 8.0f;        //Time before spawner may spawn another Enemy

    private float cooldownTimer = 0.0f;

    public void Start()
    {
        if (enemyParentGameObject == null)
        {
            enemyParentGameObject = GameObject.Find("Enemies").transform;
            if(enemyParentGameObject == null)
            {
                Debug.LogError(transform.name + " could not find \"EnemyParentGameObject\"");
            }
        }
    }

    public void Update()
    {
        CooldownTimerRoutine();
    }

    public void ResetCooldown()
    {
        cooldownTimer = 0.0f;
    }
    private void CooldownTimerRoutine()
    {
        if (cooldownTimer < 0.0f)
        {
            cooldownTimer = 0.0f;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public float DistanceFromObject(GameObject obj)
    {
        return (obj.transform.position - this.transform.position).sqrMagnitude;
    }

    public bool SpawnEnemy(GameObject prefab, GameObject player)
    {
        //If spawner still on cooldown or player to close to enemy
        if (cooldownTimer > 0.0f) return false;
        if (DistanceFromObject(player) < minDistanceToPlayer) return false;

        Vector3 spawnPosition = spawnLocation.position + RandomVector3Range(spread, -spread);
        GameObject inst = Instantiate(prefab, spawnPosition, Quaternion.identity);

        inst.gameObject.transform.SetParent(enemyParentGameObject);
        return true;
    }

    private Vector3 RandomVector3Range(Vector3 a, Vector3 b)
    {
        float x = Random.Range(a.x, b.x);
        float y = Random.Range(a.y, b.y);
        float z = Random.Range(a.z, b.z);
        return new Vector3(x, y, z);
    }

    public Transform GetSpawnLocation()
    {
        return spawnLocation;
    }
}
