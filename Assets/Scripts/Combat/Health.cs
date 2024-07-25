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
            MissionObjective
            // Add more material types as needed
        }

        public Combatants combatants;
        public int health = 100;

        private void Start()
        {
            if (combatants == Combatants.None)
            {
                switch (gameObject.tag)
                {
                    case "Player":
                        combatants = Combatants.Player;
                        break;
                    case "EnemyAI":
                        combatants = Combatants.EnemyAI;
                        break;
                    case "FriendlyAI":
                        combatants = Combatants.FriendlyAI;
                        break;
                    case "Tower":
                        combatants = Combatants.Tower;
                        break;
                    case "MissionObjective":
                        combatants = Combatants.MissionObjective;
                        break;
                }
            }
        }

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
            else if (combatants == Combatants.MissionObjective)
            {
                CountObjectiveDestroyed();
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

            if (state == true)
            {
                health = 100;
            }
        }

        private void CountEnemyDeath()
        {
            EnemySpawner.Instance.KillEnemy(gameObject);
            Destroy(gameObject);
        }

        public void CountObjectiveDestroyed()
        {
            MissionProgressHandler.instance.MissionObjectiveDestroyed(gameObject);
            Destroy(gameObject);
        }


        public bool IsPlayerDead()
        {
            return health <= 0f;
        }
    }
}
