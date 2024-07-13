using System.Collections.Generic;
using DD.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace DD.Core.AI
{
    public class EnemyAIController : MonoBehaviour
    {
        public float detectionRadius = 10f; // Detection radius for enemies
        public float fireRate = 1f; // Rate of firing bullets
        public int damage = 10; // Damage dealt per shot
        public GameObject bulletPrefab; // Prefab of the bullet to be shot
        public Transform firePoint; // The point from which the bullet will be shot
        public List<Health.Combatants> targets;
        private float stoppingDistance = 7f;

        private Transform goalDestination;
        private Vector3 temporaryDestination;
        NavMeshAgent navMeshAgent;

        public Health target;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }




        private float fireCooldown = 0f;

        void Update()
        {
            DetectTargets();
            Fight();
            MoveTowardsDestination();
        }

        void DetectTargets()
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
                    }
                }

                else
                {
                    temporaryDestination = Vector3.zero;
                    target = null;
                }

            }
        }

        void MoveTowardsDestination()
        {
            if (temporaryDestination == Vector3.zero)
            {
                // Rotate to face the current destination
                Vector3 direction = (goalDestination.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);

                // Move towards the destination
                if (Vector3.Distance(transform.position, goalDestination.position) > stoppingDistance)
                {
                    navMeshAgent.SetDestination(goalDestination.position);
                }
            }
            else
            {
                // Rotate to face the current destination
                Vector3 direction = (temporaryDestination - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);

                // Move towards the destination
                if (Vector3.Distance(transform.position, temporaryDestination) > stoppingDistance)
                {
                    navMeshAgent.SetDestination(temporaryDestination);

                }
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
