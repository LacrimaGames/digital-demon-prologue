using System;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public class LevelModifier : MonoBehaviour
    {
        public static LevelModifier instance; // Singleton instance
        [Header("Sandbox")]
        public bool sandboxMode = false;
        public bool noRewardsThisTurn = false;


        [Header("Enemy Settings")]
        // Variables to hold level data
        private int numEnemiesToSpawn;
        public int delayBeforeMissionStart;
        [Range(1, 10)]
        public int difficulty; // Placeholder, for now it's how many enemies per wave

        public int amountOfTier1Enemies;
        public int amountOfTier2Enemies;
        public int amountOfTier3Enemies;

        public float spawnInterval;

        public enum Buildings
        {
            Market,
            StonePlantation,
            Tower
        }

        [Header("Builder Settings")]
        public List<Buildings> enabledBuildings = new List<Buildings>();

        [Header("Reward 1")]
        public RewardButton.Rewards reward1;

        [Header("Reward 2")]
        public RewardButton.Rewards reward2;

        [Header("Reward 3")]
        public RewardButton.Rewards reward3;


        [Header("Core Gameobject")]
        public GameObject core;

        private void Awake()
        {
            // Ensure there is only one instance of LevelLoader
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple instances of LevelLoader found. Destroying this instance.");
                Destroy(gameObject);
            }

            Instantiate(core);
        }

        private void Start()
        {
            numEnemiesToSpawn += amountOfTier1Enemies;
            numEnemiesToSpawn += amountOfTier2Enemies;
            numEnemiesToSpawn += amountOfTier3Enemies;
        }

        // Example method to load number of enemies to spawn from a save file or configuration
        public int LoadNumEnemiesToSpawn()
        {
            // Replace with actual loading logic
            return numEnemiesToSpawn; // Default value
        }
        public int LoadDelayBeforeMissionStart()
        {
            // Replace with actual loading logic
            return delayBeforeMissionStart; // Default value
        }
        public int LoadDifficulty()
        {
            // Replace with actual loading logic
            return difficulty; // Default value
        }

        public float LoadSpawnInterval()
        {
            // Replace with actual loading logic
            return spawnInterval; // Default value
        }

        public RewardButton.Rewards LoadReward1()
        {
            return reward1;
        }

        public RewardButton.Rewards LoadReward2()
        {
            return reward2;
        }

        public RewardButton.Rewards LoadReward3()
        {
            return reward3;
        }
    }

}

