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
            Destroy(gameObject);
            if(gameObject.GetComponent<Health>().combatants == Combatants.EnemyAI)
            {
                EnemySpawner.Instance.KillEnemy(gameObject);
            }
        }
    }
}
