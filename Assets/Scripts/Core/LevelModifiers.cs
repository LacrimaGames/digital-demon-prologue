using UnityEngine;

namespace DD.Core
{
    public class LevelModifier : MonoBehaviour
    {
        public static LevelModifier instance; // Singleton instance

        // Variables to hold level data
        public int numEnemiesToSpawn;
        public int delayBeforeMissionStart;
        [Range(1,3)]
        public int difficulty; // Placeholder, for now it's how many enemies per wave

        public GameObject[] availableBuildings;

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

        // Example method to load available buildings from a database or configuration
        public GameObject[] LoadAvailableBuildings()
        {
            // Replace with actual loading logic
            return new GameObject[] { /* List of available buildings */ };
        }
    }

}

