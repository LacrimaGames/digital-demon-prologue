using UnityEngine;

namespace DD.Combat
{
    public class Healthbar : MonoBehaviour
    {
        public GameObject currentHealthObject;
        public GameObject healthBar;
        private Health health;
        private float maxHealth;
        private float currentHealth;
        private float fullLifeInPercentage;

        private void Start()
        {
            health = transform.root.GetComponent<Health>();

            if (health == null)
            {
                health = transform.parent.GetComponent<Health>();
            }

            maxHealth = health.health;
            fullLifeInPercentage = currentHealthObject.transform.localScale.x;
            currentHealthObject.SetActive(false);
        }

        private void Update()
        {
            switch (currentHealth == maxHealth)
            {
                case true:
                    healthBar.SetActive(false);
                    break;
                default:
                    healthBar.SetActive(true);
                    break;
            }

            transform.rotation = Camera.main.transform.rotation;
            currentHealth = health.health;

            Vector3 healthbarScale = currentHealthObject.transform.localScale;
            healthbarScale.x = CalculateHealth();
            currentHealthObject.transform.localScale = healthbarScale;
        }

        private float CalculateHealth()
        {
            if (currentHealth <= 0)
            {
                currentHealthObject.SetActive(false);
                return 0;
            }
            currentHealthObject.SetActive(true);
            return fullLifeInPercentage / maxHealth * currentHealth;
        }
    }
}
