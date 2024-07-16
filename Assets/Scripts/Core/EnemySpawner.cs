using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DD.Core
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public Transform[] spawnPoints; // An array of spawn points
        public Transform endGoal;

        private int enemiesPerWave; // Number of enemies to spawn per wave / Difficulty
        public float spawnDelay = 1f; // Delay between spawning each enemy within a wave

        private float spawnInterval = 20f; // Time interval between waves

        private float countdownTime;
        private float delayBeforeMissionStart = 10f; // Initial delay before spawning begins
        private int maxEnemiesThisMission = 3;
        private int currentKillCount = 0;
        private float currentSpawnCount = 0;

        private TMP_Text textTimeUntilSpawn;

        private List<GameObject> enemySpawned = new List<GameObject>();

        public static EnemySpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if(textTimeUntilSpawn == null)  
            {
                textTimeUntilSpawn = GameObject.FindGameObjectWithTag("Countdown").GetComponent<TMP_Text>();
            }
            maxEnemiesThisMission = LevelModifier.instance.LoadNumEnemiesToSpawn();
            enemiesPerWave = LevelModifier.instance.LoadDifficulty();
            delayBeforeMissionStart = LevelModifier.instance.LoadDelayBeforeMissionStart();
            spawnInterval /= LevelModifier.instance.LoadDifficulty();
            StartCoroutine(CountdownToStart());

        }

        private IEnumerator CountdownToStart()
        {
            countdownTime = delayBeforeMissionStart;

            while (countdownTime > 0)
            {
                textTimeUntilSpawn.text = countdownTime.ToString();

                yield return new WaitForSeconds(1);

                countdownTime--;
                textTimeUntilSpawn.text = countdownTime.ToString();
            }

            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(spawnDelay); // Wait for the delay before spawning the next enemy
                }
                yield return new WaitForSeconds(spawnInterval); // Wait for the next wave
            }
        }

        private void SpawnEnemy()
        {
            if (currentKillCount >= maxEnemiesThisMission)
            {
                Debug.Log("You won");
                Destroy(gameObject);
            }
            if (currentSpawnCount >= maxEnemiesThisMission)
            {
                Debug.Log("Max enemies have been spawned");
                return;
            }

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            enemySpawned.Add(enemy);
            currentSpawnCount++;
        }

        public void KillEnemy(GameObject enemy)
        {
            enemySpawned.Remove(enemy);
            currentKillCount++;
        }
    }
}
