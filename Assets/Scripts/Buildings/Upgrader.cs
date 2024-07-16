using UnityEngine;

namespace DD.Bilder
{
    public class Upgrader : MonoBehaviour
    {
        public GameObject upgradePrefab; // The new upgraded prefab
        public GameObject upgradeButton; 

        private void Start() {
            if(upgradePrefab == null)
            {
                Destroy(upgradeButton);
                Destroy(GetComponent<Upgrader>());
            }
        }

        private void OnMouseDown()
        {
            ReplacePrefab();
        }

        public void ReplacePrefab()
        {
            if(GetComponent<Upgrader>().isActiveAndEnabled == false) return;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            Instantiate(upgradePrefab, position, rotation);

            Debug.Log("Upgrade complete.");

            Destroy(gameObject);
        }
    }
}
