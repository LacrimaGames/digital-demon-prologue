using System.Collections.Generic;
using DD.Combat;
using DD.Core.AI;
using UnityEngine;

namespace DD.Core.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Health))]
    public class PlayerFighter : MonoBehaviour
    {

        [Header("Game Modifiers")]
        public float attackSpeed = 1f; // Rate of firing bullets
        public int damage = 10; // Damage dealt per shot
        public float detectionRadius = 10f; // Detection radius for enemies

        [Header("Prefabs & Objects")]
        public GameObject bulletPrefab; // Prefab of the bullet to be shot
        public Transform firePoint; // The point from which the bullet will be shot

        public List<Health.Combatants> targets;
        private PlayerController playerController;
        private float fireCooldown = 0f;
        private Health health;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            health = GetComponent<Health>();
            if (GlobalModifiers.instance != null)
            {
                GlobalModifiers.PlayerModifiers playerModifiers = GlobalModifiers.instance.LoadPlayerModifiers();
                damage = playerModifiers.attackDamage;
                attackSpeed = playerModifiers.fireRate;
                health.health = playerModifiers.health;
            }
        }

        void Update()
        {
            if (!playerController.hasWeaponEquipped) return;

            if (LevelModifier.instance.sandboxMode)
            {
                GlobalModifiers.PlayerModifiers playerModifiers = GlobalModifiers.instance.LoadPlayerModifiers();
                damage = playerModifiers.attackDamage;
                attackSpeed = playerModifiers.fireRate;
                health.health = playerModifiers.health;
            }

            fireCooldown -= Time.deltaTime;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                Health enemy = hitCollider.GetComponent<Health>();
                if (enemy != null && IsEnemyTarget(enemy) && fireCooldown <= 0f)
                {
                    Shoot(enemy);
                    fireCooldown = 1f / attackSpeed;
                    GetComponent<Animation>().Play();
                    break;
                }
            }
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

        void Shoot(Health enemy)
        {
            // Instantiate and shoot the bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(enemy, damage);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}


