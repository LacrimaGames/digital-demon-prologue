using System.Collections;
using System.Collections.Generic;
using DD.Core;
using UnityEngine;

namespace DD.Environment
{
    public class Planter : MonoBehaviour
    {
        [Header("Resources & Planter")]
        public GameObject resourcePrefab; // The tree prefab to spawn
        public Transform[] resourcePlots; // Array of transforms representing the tree plots
        public float checkInterval = 5f; // Interval (in seconds) to check for empty plots
        private List<GameObject> availableResources = new List<GameObject>();

        [Header("Upgrade")]
        public GameObject upgradeUnlock;
        public int cost;

        [Header("UI")]
        public TextMesh tooltipTextMesh; // Reference to the TextMesh component for displaying resources
        private float checkTimer;
        private Coroutine tooltip;

        void Start()
        {
            checkTimer = checkInterval;
            RespawnTrees();
            tooltipTextMesh.gameObject.SetActive(false);
        }

        void Update()
        {
            checkTimer -= Time.deltaTime;

            if (checkTimer <= 0f)
            {
                RespawnTrees();
                checkTimer = checkInterval;
            }
        }

        void RespawnTrees()
        {
            availableResources = new List<GameObject>();

            foreach (var plot in resourcePlots)
            {
                if (plot.childCount == 1)
                {
                    GameObject tree = Instantiate(resourcePrefab, plot.position, plot.rotation, plot);
                    availableResources.Add(tree);
                }
                else
                {
                    availableResources.Add(plot.GetChild(1).gameObject);
                }
            }
        }

        public List<GameObject> GetAvailableTrees()
        {
            return availableResources;
        }


        private void OnMouseDown()
        {
            if (upgradeUnlock.activeSelf == true) return;

            if (!ResourceTracker.Instance.SpendGold(cost))
            {
                if (tooltip == null)
                {
                    tooltip = StartCoroutine(ShowToolTip());
                }
            }
            else
            {
                upgradeUnlock.SetActive(true);
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var plot in resourcePlots)
            {
                Gizmos.DrawWireCube(plot.position, new Vector3(1, 1, 1));
            }
        }
    }
}
