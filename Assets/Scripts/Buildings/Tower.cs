using System.Collections.Generic;
using DD.Combat;
using DD.Core;
using UnityEngine;

namespace DD.Builder.Buildings
{
    public class Tower : MonoBehaviour
    {

        [Header("Tower Modifiers")]
        [Range(1, 3)]
        public int tier;

        private float detectionRadius = 10f; // Detection radius for enemies
        private float fireRate = 1f; // Rate of firing bullets
        private int damage = 10; // Damage dealt per shot

        [Header("Prefabs & Functions")]

        public GameObject bulletPrefab; // Prefab of the bullet to be shot
        public Transform firePoint; // The point from which the bullet will be shot
        public List<Health.Combatants> targets;

        private float fireCooldown = 0f;

        private void Start()
        {

            if (GlobalModifiers.instance != null)
            {
                switch (tier)
                {
                    case 1:
                        GlobalModifiers.Tier1TowerModifiers tier1Modifiers = GlobalModifiers.instance.LoadTier1TowerModifiers();
                        detectionRadius = tier1Modifiers.detectionRadius;
                        fireRate = tier1Modifiers.fireRate;
                        damage = tier1Modifiers.damage;
                        break;
                    case 2:
                        GlobalModifiers.Tier2TowerModifiers tier2Modifiers = GlobalModifiers.instance.LoadTier2TowerModifiers();
                        detectionRadius = tier2Modifiers.detectionRadius;
                        fireRate = tier2Modifiers.fireRate;
                        damage = tier2Modifiers.damage;
                        break;
                    case 3:
                        GlobalModifiers.Tier3TowerModifiers tier3Modifiers = GlobalModifiers.instance.LoadTier3TowerModifiers();
                        detectionRadius = tier3Modifiers.detectionRadius;
                        fireRate = tier3Modifiers.fireRate;
                        damage = tier3Modifiers.damage;
                        break;
                }
            }


        }

        void Update()
        {
            fireCooldown -= Time.deltaTime;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                Health enemy = hitCollider.GetComponent<Health>();
                if (enemy != null && IsEnemyTarget(enemy) && fireCooldown <= 0f)
                {
                    Shoot(enemy);
                    fireCooldown = 1f / fireRate;
                    break;
                }
            }
        }

        void Shoot(Health enemy)
        {
            // Instantiate and shoot the bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemy, damage);
        }

        bool IsEnemyTarget(Health attemptedTarget)
        {
            foreach (Health.Combatants target in targets)
            {
                if (attemptedTarget.combatants == target)
                {
                    return true;
                }
            }
            return false;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}


