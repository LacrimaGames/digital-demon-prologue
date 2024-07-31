using System.Collections;
using DD.Builder.Buildings;
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
        public bool canUpgrade = true;
        public GameObject acceptButton;
        public GameObject denyButton;

        private void Start()
        {
            if (!canUpgrade)
            {
                upgradeButton.SetActive(false);
            }
            else
            {
                upgradeButton.SetActive(true);
                acceptButton = upgradeButton.transform.GetChild(0).gameObject;
                denyButton = upgradeButton.transform.GetChild(1).gameObject;
                upgradeButton.transform.rotation = Camera.main.transform.rotation;
            }

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
                if (hit.collider.gameObject == upgradeButton && Input.GetMouseButtonDown(0))
                {
                    if(acceptButton.activeSelf)
                    {
                        upgradeButton.GetComponent<Animation>().clip = upgradeButton.GetComponent<Animation>().GetClip("CloseUpgradeTooltip");
                        upgradeButton.GetComponent<Animation>().Play();
                    }
                    else
                    {
                        upgradeButton.GetComponent<Animation>().clip = upgradeButton.GetComponent<Animation>().GetClip("OpenUpgradeTooltip");
                        upgradeButton.GetComponent<Animation>().Play();
                    }
                }
                if (hit.collider.gameObject == acceptButton && Input.GetMouseButtonDown(0))
                {
                    ReplacePrefab();
                }
                if (hit.collider.gameObject == denyButton && Input.GetMouseButtonDown(0))
                {
                    upgradeButton.GetComponent<Animation>().clip = upgradeButton.GetComponent<Animation>().GetClip("CloseUpgradeTooltip");
                    upgradeButton.GetComponent<Animation>().Play();
                }
                else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    upgradeButton.GetComponent<Animation>().clip = upgradeButton.GetComponent<Animation>().GetClip("CloseUpgradeTooltip");
                    upgradeButton.GetComponent<Animation>().Play();
                }
            }
        }

        public void ReplacePrefab()
        {
            if (GetComponent<Upgrader>().isActiveAndEnabled == false) return;

            if (Input.GetMouseButton(0))
            {
                if (replacesOldBuilding)
                {
                    if (ResourceTracker.Instance.SpendGold(cost) || LevelModifier.instance.sandboxMode)
                    {
                        Vector3 position = transform.position;
                        Quaternion rotation = transform.rotation;

                        GameObject newObject = Instantiate(upgradePrefab, position, rotation);

                        if (newObject.tag == "Tower")
                        {
                            newObject.GetComponent<Tower>().attackPriority = GetComponent<Tower>().attackPriority;
                        }

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
