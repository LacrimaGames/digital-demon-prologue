using System.Collections;
using DD.Core;
using UnityEngine;

namespace DD.Bilder
{
    public class Upgrader : MonoBehaviour
    {
        public GameObject upgradePrefab; // The new upgraded prefab
        public GameObject upgradeButton;
        public int cost;

        public LayerMask upgradeMask;

        [Header("UI")]
        public TextMesh tooltipTextMesh; // Reference to the TextMesh component for displaying resources
        private Coroutine tooltip;
        public bool replacesOldBuilding = true;

        private void Start()
        {
            if (upgradePrefab == null)
            {
                Destroy(upgradeButton);
                Destroy(GetComponent<Upgrader>());
            }
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, upgradeMask))
            {
                if(hit.collider.gameObject == upgradeButton)
                {
                    ReplacePrefab();
                }
            }
        }

        public void ReplacePrefab()
        {
            if (GetComponent<Upgrader>().isActiveAndEnabled == false) return;

            if(Input.GetMouseButton(0))
            {
                if (replacesOldBuilding)
                {
                    if (ResourceTracker.Instance.SpendGold(cost))
                    {
                        Vector3 position = transform.position;
                        Quaternion rotation = transform.rotation;

                        Instantiate(upgradePrefab, position, rotation);

                        Debug.Log("Upgrade complete.");

                        Destroy(gameObject);
                    }
                    else
                    {
                        tooltip = StartCoroutine(ShowToolTip());
                    }
                }
                else
                {
                    if (ResourceTracker.Instance.SpendGold(cost))
                    {
                        upgradePrefab.SetActive(true);
                        Debug.Log("Upgrade complete.");
                        Destroy(upgradeButton);
                        Destroy(GetComponent<Upgrader>());
                    }
                    else
                    {
                        tooltip = StartCoroutine(ShowToolTip());
                    }
                }
            }
        }

        private IEnumerator ShowToolTip()
        {
            tooltipTextMesh.gameObject.SetActive(true);
            // Update the tooltip text with the current resources in the storage
            tooltipTextMesh.text = $"Upgrade costs {cost} Gold";
            //     // Make the tooltip face the camera
            tooltipTextMesh.transform.rotation = Camera.main.transform.rotation;
            yield return new WaitForSeconds(2);
            tooltipTextMesh.gameObject.SetActive(false);
            tooltip = null;
        }
    }
}
