using UnityEngine;

namespace DD.Core
{
    public class GlobalModifiers : MonoBehaviour
    {
        public static GlobalModifiers instance; // Singleton instance

        [System.Serializable]
        public class PlayerModifiers
        {
            public float gatheringSpeedPerSecond = 1f; // Time between each gathering action
            public int maxAmountHeld = 20;
            public float unloadSpeedPerSecond = 1; // Speed of unloading materials (units per second)
            public int attackDamage = 10;
            public float movementSpeed = 5f;
        }

        [System.Serializable]
        public class FriendlyAIModifiers
        {
            public float gatheringSpeedPerSecond = 1f; // Time between each gathering action
            public int maxAmountHeld = 20;
            public float unloadSpeedPerSecond = 1; // Speed of unloading materials (units per second)
            public int unloadAmount = 1; // Amount of materials unloaded per action
            public float movementSpeed = 5f;
        }

        [System.Serializable]
        public class Tier1TowerModifiers
        {
            public float detectionRadius = 10f; // Detection radius for enemies
            public float fireRate = 1f; // Rate of firing bullets
            public int damage = 10; // Damage dealt per shot
        }

        [System.Serializable]
        public class Tier2TowerModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 2f; // Rate of firing bullets
            public int damage = 15; // Damage dealt per shot
        }

        [System.Serializable]
        public class Tier3TowerModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 3f; // Rate of firing bullets
            public int damage = 25; // Damage dealt per shot
        }

        public PlayerModifiers playerModifiers;
        public FriendlyAIModifiers friendlyAIModifiers;
        public Tier1TowerModifiers tier1TowerModifiers;
        public Tier2TowerModifiers tier2TowerModifiers;
        public Tier3TowerModifiers tier3TowerModifiers;


        // Variables to hold level data

        private void Awake()
        {
            // Ensure there is only one instance of LevelLoader
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("Multiple instances of LevelLoader found. Destroying this instance.");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerModifiers.maxAmountHeld += 10;
            }
        }

        public PlayerModifiers LoadPlayerModifiers()
        {
            // Replace with actual loading logic
            return playerModifiers; // Default value
        }

        public FriendlyAIModifiers LoadFriendlyAIModifiers()
        {
            // Replace with actual loading logic
            return friendlyAIModifiers; // Default value
        }

        public Tier1TowerModifiers LoadTier1TowerModifiers()
        {
            // Replace with actual loading logic
            return tier1TowerModifiers; // Default value
        }

        public Tier2TowerModifiers LoadTier2TowerModifiers()
        {
            // Replace with actual loading logic
            return tier2TowerModifiers; // Default value
        }

        public Tier3TowerModifiers LoadTier3TowerModifiers()
        {
            // Replace with actual loading logic
            return tier3TowerModifiers; // Default value
        }
    }

}

