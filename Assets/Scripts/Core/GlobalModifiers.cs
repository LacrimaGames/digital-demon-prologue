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
            public int maxAmountHeld = 20; // Max amount player can carry
            public float unloadSpeedPerSecond = 1; // Speed of unloading materials (units per second)
            public int attackDamage = 10; // Damage dealt per shot
            public float fireRate = 10; // Rate of firing bullets per second
            public float movementSpeed = 5f;
            public int health = 100;
        }

        [System.Serializable]
        public class FriendlyAIModifiers
        {
            public float gatheringSpeedPerSecond = 1f; // Time between each gathering action
            public int maxAmountHeld = 20; // Max amount AI can carry
            public float unloadSpeedPerSecond = 1; // Speed of unloading materials (units per second)
            public int unloadAmount = 1; // Amount of materials unloaded per action
            public float movementSpeed = 5f;
            public int health = 100;
        }

        [System.Serializable]
        public class Tier1TowerModifiers
        {
            public float detectionRadius = 10f; // Detection radius for enemies
            public float fireRate = 1f; // Rate of firing bullets per second
            public int damage = 10; // Damage dealt per shot
            public int health = 100;
        }

        [System.Serializable]
        public class Tier2TowerModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 2f; // Rate of firing bullets per second
            public int damage = 15; // Damage dealt per shot
            public int health = 100;
        }

        [System.Serializable]
        public class Tier3TowerModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 3f; // Rate of firing bullets per second
            public int damage = 25; // Damage dealt per shot
            public int health = 100;
        }

        [System.Serializable]
        public class Tier1EnemyModifiers
        {
            public float detectionRadius = 10f; // Detection radius for enemies
            public float fireRate = 1f; // Rate of firing bullets
            public int damage = 10; // Damage dealt per shot
            public float movementSpeed = 5f;
            public int health = 100;
        }

        [System.Serializable]
        public class Tier2EnemyModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 2f; // Rate of firing bullets per second
            public int damage = 15; // Damage dealt per shot
            public float movementSpeed = 5f;
            public int health = 100;
        }

        [System.Serializable]
        public class Tier3EnemyModifiers
        {
            public float detectionRadius = 20f; // Detection radius for enemies
            public float fireRate = 3f; // Rate of firing bullets per second
            public int damage = 25; // Damage dealt per shot
            public float movementSpeed = 5f;
            public int health = 100;
        }

        [Header("Player Modifiers")]
        public PlayerModifiers playerModifiers;

        [Header("Friendly AI Modifiers")]
        public FriendlyAIModifiers friendlyAIModifiers;

        [Header("Tower Modifiers")]
        public Tier1TowerModifiers tier1TowerModifiers;
        public Tier2TowerModifiers tier2TowerModifiers;
        public Tier3TowerModifiers tier3TowerModifiers;

        [Header("Enemy AI Modifiers")]
        public Tier1EnemyModifiers tier1EnemyModifiers;
        public Tier2EnemyModifiers tier2EnemyModifiers;
        public Tier3EnemyModifiers tier3EnemyModifiers;

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

        public Tier1EnemyModifiers LoadTier1EnemyModifiers()
        {
            // Replace with actual loading logic
            return tier1EnemyModifiers; // Default value
        }

        public Tier2EnemyModifiers LoadTier2EnemyModifiers()
        {
            // Replace with actual loading logic
            return tier2EnemyModifiers; // Default value
        }

        public Tier3EnemyModifiers LoadTier3EnemyModifiers()
        {
            // Replace with actual loading logic
            return tier3EnemyModifiers; // Default value
        }
    }

}

