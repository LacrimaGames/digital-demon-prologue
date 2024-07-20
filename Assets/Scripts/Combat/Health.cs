using System;
using System.Collections;
using DD.Core;
using UnityEngine;

namespace DD.Combat
{
    public class Health : MonoBehaviour
    {
        public enum Combatants
        {
            None,
            Player,
            EnemyAI,
            FriendlyAI,
            Tower,
            Wall,
            // Add more material types as needed
        }

        public Combatants combatants;
        public int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            if (combatants == Combatants.Player)
            {
                StartCoroutine(RespawnPlayer());
            }
            else if (combatants == Combatants.EnemyAI)
            {
                CountEnemyDeath();
            }
        }

        private IEnumerator RespawnPlayer()
        {
            ComponentControl(false);
            yield return new WaitForSeconds(5f);
            ComponentControl(true);
        }
    
        private void ComponentControl(bool state)
        {
            GetComponent<Collider>().enabled = state;

            foreach (var item in GetComponents<MonoBehaviour>())
            {
                item.enabled = state;
            }

            if(state == true)
            {
                health = 100;
            }
        }

        private void CountEnemyDeath()
        {
            EnemySpawner.Instance.KillEnemy(gameObject);
            Destroy(gameObject);
        }

        public bool IsPlayerDead()
        {
            return health <= 0f;
        }
    }
}
