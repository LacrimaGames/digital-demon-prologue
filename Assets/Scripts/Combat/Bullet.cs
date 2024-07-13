using DD.Core.AI;
using UnityEngine;

namespace DD.Combat
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f; // Speed of the bullet
        private Health target;
        private int damage;

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;
            float distanceThisFrame = speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.transform.position) <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            // Move the bullet and rotate it to face the direction of travel
            transform.Translate(direction * distanceThisFrame, Space.World);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        public void SetTarget(Health enemy, int damageAmount)
        {
            target = enemy;
            damage = damageAmount;
        }

        void HitTarget()
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
