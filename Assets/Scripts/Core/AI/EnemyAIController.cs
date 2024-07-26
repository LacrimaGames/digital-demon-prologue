using System.Collections.Generic;
using DD.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace DD.Core.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        [Header("Modifiers")]
        public float detectionRadius = 10f; // Detection radius for enemies
        public float fireRate = 1f; // Rate of firing bullets
        public int damage = 10; // Damage dealt per shot
        [Range(1, 3)]
        public int tier;

        [Header("Targets")]
        public List<Health.Combatants> targets;

        [Header("Prefabs & Objects")]
        public GameObject bulletPrefab; // Prefab of the bullet to be shot
        public Transform firePoint; // The point from which the bullet will be shot
        public Transform goalDestination;

        private Vector3 temporaryDestination;
        private NavMeshAgent navMeshAgent;
        private Health target;
        private Health health;

        private float fireCooldown = 0f;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();

            if (GlobalModifiers.instance != null)
            {
                switch (tier)
                {
                    case 1:
                        GlobalModifiers.Tier1EnemyModifiers tier1Modifiers = GlobalModifiers.instance.LoadTier1EnemyModifiers();
                        detectionRadius = tier1Modifiers.detectionRadius;
                        fireRate = tier1Modifiers.fireRate;
                        damage = tier1Modifiers.damage;
                        navMeshAgent.speed = tier1Modifiers.movementSpeed;
                        health.health = tier1Modifiers.health;
                        break;
                    case 2:
                        GlobalModifiers.Tier2EnemyModifiers tier2Modifiers = GlobalModifiers.instance.LoadTier2EnemyModifiers();
                        detectionRadius = tier2Modifiers.detectionRadius;
                        fireRate = tier2Modifiers.fireRate;
                        damage = tier2Modifiers.damage;
                        navMeshAgent.speed = tier2Modifiers.movementSpeed;
                        health.health = tier2Modifiers.health;
                        break;
                    case 3:
                        GlobalModifiers.Tier3EnemyModifiers tier3Modifiers = GlobalModifiers.instance.LoadTier3EnemyModifiers();
                        detectionRadius = tier3Modifiers.detectionRadius;
                        fireRate = tier3Modifiers.fireRate;
                        damage = tier3Modifiers.damage;
                        navMeshAgent.speed = tier3Modifiers.movementSpeed;
                        health.health = tier3Modifiers.health;
                        break;
                }
            }

            if (goalDestination == null)
            {
                goalDestination = EnemySpawner.Instance.endGoal;
            }
        }

        void Update()
        {
            DetectTargets();
            MoveTowardsDestination();
        }

        private void DetectTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                Health enemy = hitCollider.GetComponent<Health>();

                if (Vector3.Distance(transform.position, hitCollider.transform.position) <= detectionRadius)
                {
                    if (enemy != null && IsEnemyTarget(enemy))
                    {
                        temporaryDestination = enemy.transform.position;
                        target = enemy;
                        Fight();
                    }
                    else
                    {
                        temporaryDestination = Vector3.zero;
                    }
                }
                else
                {
                    target = null;
                }
            }
        }

        private void MoveTowardsDestination()
        {
            if (temporaryDestination == Vector3.zero)
            {
                // Rotate to face the current destination
                Vector3 direction = (goalDestination.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(transform.forward);
                navMeshAgent.SetDestination(goalDestination.position);
            }
            else
            {
                // Rotate to face the current destination
                Vector3 direction = (temporaryDestination - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(transform.forward);

                // Move towards the destination

                navMeshAgent.SetDestination(temporaryDestination);
            }

        }

        private void Fight()
        {
            fireCooldown -= Time.deltaTime;

            if (target != null && IsEnemyTarget(target))
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);

                if (fireCooldown <= 0f)
                {
                    Shoot(target);
                    fireCooldown = 1f / fireRate;
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
