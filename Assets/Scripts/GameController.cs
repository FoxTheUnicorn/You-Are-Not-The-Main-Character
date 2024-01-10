using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private UnityEvent OnWaveOver;

    [SerializeField] private GameObject Player;
    [SerializeField] private Transform EnemiesParent;
    [SerializeField] private Transform EnemySpawnerParent;
    [SerializeField] private List<EnemySpawner> EnemySpawnerList = new List<EnemySpawner>();
    [SerializeField] private List<EnemyWave> Waves = new List<EnemyWave>();


    [SerializeField] private EnemyWave currentWave;


    // Start is called before the first frame update
    void Start()
    {
        ResolveEnemySpawners();
        StartWave(1);
        StartGameLoop();
    }

    public void StartWave(int wave)
    {
        inWave = true;
        currentWave = new EnemyWave(Waves[wave-1]);
    }

    private void StartGameLoop()
    {
        InvokeRepeating("GameLoop", 0.0f, 0.5f);
    }

    private void StopGameLoop()
    {
        CancelInvoke();
    }

    private void GameLoop()
    {
        foreach (EnemySpawner eSpawn in EnemySpawnerList)
        {
            GameObject enemy = GetEnemyFromCurrentWave();
            if(enemy != null)
            {
                eSpawn.SpawnEnemy(enemy, Player);
            }
            else
            {
                OnWaveOver.Invoke();
                break;
            }
        }
    }

    private GameObject GetEnemyFromCurrentWave()
    {
        if (currentWave.Enemies.Count == 0) return null;
        GameObject EnemyType = currentWave.Enemies[0].EnemyType;
        currentWave.Enemies[0].Spawn();
        if (currentWave.Enemies[0].GetRemainingSpawns() < 0)
        {
            currentWave.Enemies.RemoveAt(0);
        }
        return EnemyType;
    }

    private void ResolveEnemySpawners()
    {
        for (int i = 0; i < EnemySpawnerParent.childCount; i++)
        {
            EnemySpawnerList.Add(EnemySpawnerParent.GetChild(i).GetComponent<EnemySpawner>());
        }
    }

    [System.Serializable]
    class EnemyWave {
        public List<EnemyWaveEntry> Enemies;

        public EnemyWave(EnemyWave EnemyWave)
        {
            this.Enemies = EnemyWave.Enemies;
        }
        public EnemyWave(List<EnemyWaveEntry> enemies)
        {
            this.Enemies = enemies;
        }
    }

    [System.Serializable]
    class EnemyWaveEntry
    {
        public GameObject EnemyType;
        [Range(1, 50)] public int Amount;
        private int Spawned;
        public EnemyWaveEntry(GameObject EnemyType, int Amount)
        {
            this.EnemyType = EnemyType;
            this.Amount = Amount;
            this.Spawned = 0;
        }

        public int GetRemainingSpawns()
        {
            return Amount - Spawned;
        }

        public void Spawn()
        {
            this.Spawned = Spawned + 1;
        }
    }
}
