using System;
using System.Collections.Generic;
using DD.Combat;
using DD.Core;
using DD.Core.AI;
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
        public List<Health.Combatants> viableTargets;

        public List<Health> targets = new List<Health>();

        private float fireCooldown = 0f;
        private Health health;

        public GameObject tooltipCollection;

        public enum Priority
        {
            LowestHealth,
            HighestTier,
            Closest
        }

        public Priority attackPriority;

        private void Start()
        {
            tooltipCollection.transform.rotation = Camera.main.transform.rotation;
            health = GetComponent<Health>();

            if (GlobalModifiers.instance != null)
            {
                switch (tier)
                {
                    case 1:
                        GlobalModifiers.Tier1TowerModifiers tier1Modifiers = GlobalModifiers.instance.LoadTier1TowerModifiers();
                        detectionRadius = tier1Modifiers.detectionRadius;
                        fireRate = tier1Modifiers.fireRate;
                        damage = tier1Modifiers.damage;
                        health.health = tier1Modifiers.health;
                        break;
                    case 2:
                        GlobalModifiers.Tier2TowerModifiers tier2Modifiers = GlobalModifiers.instance.LoadTier2TowerModifiers();
                        detectionRadius = tier2Modifiers.detectionRadius;
                        fireRate = tier2Modifiers.fireRate;
                        damage = tier2Modifiers.damage;
                        health.health = tier2Modifiers.health;
                        break;
                    case 3:
                        GlobalModifiers.Tier3TowerModifiers tier3Modifiers = GlobalModifiers.instance.LoadTier3TowerModifiers();
                        detectionRadius = tier3Modifiers.detectionRadius;
                        fireRate = tier3Modifiers.fireRate;
                        damage = tier3Modifiers.damage;
                        health.health = tier3Modifiers.health;
                        break;
                }
            }


        }

        void Update()
        {
            if(LevelModifier.instance.sandboxMode)
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
                            health.health = tier1Modifiers.health;
                            break;
                        case 2:
                            GlobalModifiers.Tier2TowerModifiers tier2Modifiers = GlobalModifiers.instance.LoadTier2TowerModifiers();
                            detectionRadius = tier2Modifiers.detectionRadius;
                            fireRate = tier2Modifiers.fireRate;
                            damage = tier2Modifiers.damage;
                            health.health = tier2Modifiers.health;
                            break;
                        case 3:
                            GlobalModifiers.Tier3TowerModifiers tier3Modifiers = GlobalModifiers.instance.LoadTier3TowerModifiers();
                            detectionRadius = tier3Modifiers.detectionRadius;
                            fireRate = tier3Modifiers.fireRate;
                            damage = tier3Modifiers.damage;
                            health.health = tier3Modifiers.health;
                            break;
                    }
                }
            }
            
            fireCooldown -= Time.deltaTime;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                Health enemy = hitCollider.GetComponent<Health>();

                if (enemy != null && IsEnemyTarget(enemy))
                {
                    if(!targets.Contains(enemy))
                    {
                        targets.Add(enemy);
                    }

                    enemy = DeterminePriority();



                    if (fireCooldown <= 0f)
                    {
                        Shoot(enemy);
                        fireCooldown = 1f / fireRate;
                        break;
                    }
                }
            }
        }

        Health DeterminePriority()
        {
            Health enemyToPrioritize = null;
            CheckForCasualities();
            if (attackPriority == Priority.LowestHealth)
            {
                float tempHealth = 1000f;
                foreach (Health target in targets)
                {
                    if(tempHealth < target.health) continue;
                    tempHealth = target.health;
                    enemyToPrioritize = target;
                }
            }

            if (attackPriority == Priority.HighestTier)
            {
                float tempTier = 0f;
                foreach (Health target in targets)
                {
                    if (tempTier >= target.GetComponent<EnemyAIController>().tier) continue;
                    tempTier = target.GetComponent<EnemyAIController>().tier;
                    enemyToPrioritize = target;
                }
            }

            if (attackPriority == Priority.Closest)
            {
                float tempDistance = detectionRadius;
                foreach (Health target in targets)
                {
                    if (tempDistance < Vector3.Distance(transform.position, target.transform.position)) continue;
                    tempDistance = Vector3.Distance(transform.position, target.transform.position);
                    enemyToPrioritize = target;
                }
            }

            return enemyToPrioritize;
        }

        void Shoot(Health enemy)
        {
            // Instantiate and shoot the bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemy, damage);

        }

        private void CheckForCasualities()
        {

            List<Health> targetsToRemove = new List<Health>();

            foreach (Health target in targets)
            {
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > detectionRadius)
                {
                    targetsToRemove.Add(target);
                }
            }

            foreach (Health target in targetsToRemove)
            {
                targets.Remove(target);
            }
        }

        bool IsEnemyTarget(Health attemptedTarget)
        {
            foreach (Health.Combatants target in viableTargets)
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


