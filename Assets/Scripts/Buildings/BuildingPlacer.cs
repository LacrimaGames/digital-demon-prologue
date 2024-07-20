using System.Collections.Generic;
using DD.Core;
using DD.Core.Player;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

namespace DD.Builder.Buildings
{
    public class BuildingPlacer : MonoBehaviour
    {
        public Camera playerCamera;
        public LayerMask structuresPlacementGround;
        public LayerMask towerPlacementGround;
        public LayerMask placementBlocker;

        BoxCollider prefabCollider;

        [System.Serializable]
        public class BuildingData
        {
            public GameObject buildingPrefab;
            public int cost;
        }

        public List<BuildingData> buildings;

        // public GameObject[] buildingPrefabs;

        private GameObject currentPrefab;
        private GameObject prefabPreview;
        private bool isPlacing = false;
        private int currentPrefabCost = 0;

        void Update()
        {
            if(!FindFirstObjectByType<PlayerController>().isActiveAndEnabled)
            {
                CancelPlacing();
                return;
            }

            if (isPlacing)
            {
                HandlePlacement();
            }
            // Add more keys as needed for other buildings
        }

        public void StartPlacingBuilding(int prefabIndex)
        {
            if (prefabIndex >= 0 && prefabIndex < buildings.Count)
            {
                if (prefabPreview != null)
                {
                    Destroy(prefabPreview);
                }

                currentPrefab = buildings[prefabIndex].buildingPrefab;
                currentPrefabCost = buildings[prefabIndex].cost;
                prefabPreview = Instantiate(currentPrefab.GetComponent<Builder>().previewPrefab);
                prefabCollider = prefabPreview.GetComponent<BoxCollider>();
                // prefabPreview.GetComponent<Collider>().enabled = false;
                isPlacing = true;
            }
        }

        void HandlePlacement()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, structuresPlacementGround) || Physics.Raycast(ray, out hit, Mathf.Infinity, towerPlacementGround))
            {
                prefabPreview.transform.position = hit.point;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementBlocker) || !IsPlacementValid())
                {
                    Debug.Log("Cannot Place here");
                }
                else if (IsPlacementValid())
                {
                    Debug.Log("Can Place");
                }


                if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
                    if (IsPlacementValid())
                    {
                        if (ResourceTracker.Instance.SpendGold(currentPrefabCost) && !EventSystem.current.IsPointerOverGameObject())
                        {
                            PlaceBuilding();
                        }
                        else
                        {
                            Debug.Log("Not enough gold to place the building.");
                        }
                    }
                    else
                    {
                        Debug.Log("Invalid placement: The whole prefab is not on the ground.");
                    }
                }
                else if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Escape)) // Right mouse button to cancel
                {
                    CancelPlacing();
                }
            }
        }

        bool IsPlacementValid()
        {
            prefabCollider.enabled = true; // Enable collider to check for collisions

            Bounds bounds = prefabCollider.bounds;

            List<Vector3> checkPoints = new List<Vector3>
            {
                bounds.min,
                bounds.max,
                new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
                new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
                new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
                new Vector3(bounds.max.x, bounds.max.y, bounds.max.z),
                new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
                new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)
            };

            if(currentPrefab.tag == "Tower")
            {
                foreach (var point in checkPoints)
                {
                    Ray ray = new Ray(point + Vector3.up * 10f, Vector3.down); // Cast ray downwards from above the point
                    if (Physics.Raycast(ray, 20f, placementBlocker))
                    {
                        return false;
                    }
                    if (!Physics.Raycast(ray, 20f, towerPlacementGround))
                    {
                        return false;
                    }
                }
            }
            if(currentPrefab.tag == "Structure")
            {
                foreach (var point in checkPoints)
                {
                    Ray ray = new Ray(point + Vector3.up * 10f, Vector3.down); // Cast ray downwards from above the point
                    if (Physics.Raycast(ray, 20f, placementBlocker))
                    {
                        return false;
                    }
                    if (Physics.Raycast(ray, 20f, towerPlacementGround))
                    {
                        return false;
                    }
                    if (!Physics.Raycast(ray, 20f, structuresPlacementGround))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void OnDrawGizmos()
        {
            if (isPlacing && prefabCollider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(prefabCollider.bounds.center, prefabCollider.bounds.size);
            }
        }

        void PlaceBuilding()
        {
            Instantiate(currentPrefab, prefabPreview.transform.position, prefabPreview.transform.rotation);
            Destroy(prefabPreview);
            isPlacing = false;
        }

        void CancelPlacing()
        {
            if(prefabPreview != null)
            {
                Destroy(prefabPreview);
            }
            isPlacing = false;
        }
    }
}
