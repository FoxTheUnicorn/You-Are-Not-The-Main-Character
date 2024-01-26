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
    [SerializeField] private float SpawnInterval = 1.0f;
    [SerializeField] private float WaveCooldown = 10.0f;
    [SerializeField] public UIWaveController uiWaveController;

    [SerializeField] private int WaveCounter;
    [SerializeField] private EnemyWave currentWave;


    // Start is called before the first frame update
    void Start()
    {
        WaveCounter = 1;
        ResolveEnemySpawners();
        StartWave(WaveCounter);
        StartGameLoop();
    }

    public void StartWave(int wave)
    {
        currentWave = new EnemyWave(Waves[wave-1]);
        uiWaveController.StartNewWave(wave, currentWave.GetEnemyCount());
    }

    private void StartGameLoop()
    {
        InvokeRepeating("GameLoop", 0.0f, SpawnInterval);
    }

    private void StopGameLoop()
    {
        CancelInvoke();
    }

    private void GameLoop()
    {
        foreach (EnemySpawner eSpawn in EnemySpawnerList)
        {
            SpawnEnemy(eSpawn);
        }
    }

    private void SpawnEnemy(EnemySpawner spawner)
    {
        if (currentWave.Enemies.Count <= 0) return;             //If no Enemies left to spawn
        if (currentWave.Enemies[0].GetRemainingSpawns() <= 0)   //If no Enemies from current Group left to spawn
        {
            currentWave.Enemies.RemoveAt(0);
            if (currentWave.Enemies.Count <= 0) return;
        }
        bool val = spawner.SpawnEnemy(currentWave.Enemies[0].EnemyType, Player);
        if(val)
        {
            currentWave.Enemies[0].Spawn();
        }
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

        public EnemyWave(EnemyWave EnemyWave)   //Copy Constructor
        {
            this.Enemies = new List<EnemyWaveEntry>(EnemyWave.Enemies);
        }

        public EnemyWave(List<EnemyWaveEntry> enemies) 
        {
            this.Enemies = enemies;
        }

        public int GetEnemyCount()
        {
            int outVar = 0;
            foreach(EnemyWaveEntry entry in Enemies)
            {
                outVar += entry.Amount;
            }
            return outVar;
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
        public EnemyWaveEntry(EnemyWaveEntry entry) //Copy Constructor
        {
            this.EnemyType = entry.EnemyType;
            this.Amount = entry.Amount;
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
