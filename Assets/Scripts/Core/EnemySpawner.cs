using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DD.Core
{
    public class EnemySpawner : MonoBehaviour
    {

        public GameObject tier1EnemyPrefab;
        public GameObject tier2EnemyPrefab;
        public GameObject tier3EnemyPrefab;

        public Transform[] spawnPoints; // An array of spawn points
        public Transform endGoal;

        private int amountOfTier1Enemies;
        private int amountOfTier2Enemies;
        private int amountOfTier3Enemies;

        private int enemiesPerWave; // Number of enemies to spawn per wave / Difficulty
        public float spawnDelay = 1f; // Delay between spawning each enemy within a wave

        private float spawnInterval = 20f; // Time interval between waves

        private float countdownTime;
        private float delayBeforeMissionStart = 10f; // Initial delay before spawning begins
        private int maxEnemiesThisMission = 3;
        private int currentKillCount = 0;
        private float currentSpawnCount = 0;


        public float currentSpawnCountTier1 = 0;
        public float currentSpawnCountTier2 = 0;
        public float currentSpawnCountTier3 = 0;


        private TMP_Text textTimeUntilSpawn;

        private List<GameObject> enemySpawned = new List<GameObject>();

        public static EnemySpawner Instance { get; private set; }

        public bool gameStarted = false;

        public TextMesh tooltipTextMesh; // Reference to the TextMesh component for displaying resources

        private float countdownTilNextSpawn;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (textTimeUntilSpawn == null)
            {
                textTimeUntilSpawn = GameObject.FindGameObjectWithTag("Countdown").GetComponent<TMP_Text>();
            }
            maxEnemiesThisMission = LevelModifier.instance.LoadNumEnemiesToSpawn();
            enemiesPerWave = LevelModifier.instance.LoadDifficulty();
            delayBeforeMissionStart = LevelModifier.instance.LoadDelayBeforeMissionStart();
            spawnInterval /= LevelModifier.instance.LoadDifficulty();
            amountOfTier1Enemies = LevelModifier.instance.amountOfTier1Enemies;
            amountOfTier2Enemies = LevelModifier.instance.amountOfTier2Enemies;
            amountOfTier3Enemies = LevelModifier.instance.amountOfTier3Enemies;
            tooltipTextMesh.gameObject.SetActive(false);
            countdownTilNextSpawn = spawnInterval + delayBeforeMissionStart;

            StartGame();
        }

        public void StartGame()
        {
            if (!gameStarted)
            {
                StartCoroutine(CountdownToStart());
            }
        }

        public IEnumerator CountdownToStart()
        {
            countdownTime = delayBeforeMissionStart;

            while (countdownTime > 0)
            {
                textTimeUntilSpawn.text = countdownTime.ToString();

                yield return new WaitForSeconds(1);

                countdownTime--;
                textTimeUntilSpawn.text = countdownTime.ToString();
            }

            gameStarted = true;
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    if (currentSpawnCountTier1 < amountOfTier1Enemies)
                    {
                        currentSpawnCountTier1++;
                        SpawnEnemy(tier1EnemyPrefab);
                        yield return new WaitForSeconds(spawnDelay);
                    }
                    else if (currentSpawnCountTier2 < amountOfTier2Enemies)
                    {
                        currentSpawnCountTier2++;
                        SpawnEnemy(tier2EnemyPrefab);
                        yield return new WaitForSeconds(spawnDelay);
                    }
                    else if (currentSpawnCountTier3 < amountOfTier3Enemies)
                    {
                        currentSpawnCountTier3++;
                        SpawnEnemy(tier3EnemyPrefab);
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
                StartCoroutine(Countdown(spawnInterval));
                yield return new WaitForSeconds(spawnInterval); // Wait for the next wave
            }
        }

        IEnumerator Countdown(float seconds)
        {
            float counter = seconds;

            tooltipTextMesh.text = counter.ToString();
            tooltipTextMesh.transform.rotation = Camera.main.transform.rotation;
            while (counter > 0)
            {
                tooltipTextMesh.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                counter--;
                tooltipTextMesh.text = counter.ToString();
            }
        }

        private void SpawnEnemy(GameObject enemyToSpawn)
        {
            if (currentSpawnCount >= maxEnemiesThisMission)
            {
                Debug.Log("Max enemies have been spawned");
                tooltipTextMesh.gameObject.SetActive(false);
                return;
            }

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            enemySpawned.Add(enemy);
            currentSpawnCount++;
        }

        public void KillEnemy(GameObject enemy)
        {
            enemySpawned.Remove(enemy);
            currentKillCount++;
        }

        public int GetCurrentKillCount()
        {
            return currentKillCount;
        }

        public int GetMaxEnemiesThisMission()
        {
            return maxEnemiesThisMission;
        }
    }
}
