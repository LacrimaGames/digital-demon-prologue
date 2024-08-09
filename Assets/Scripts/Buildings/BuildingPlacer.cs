using System.Collections;
using System.Collections.Generic;
using DD.Core;
using DD.Core.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DD.Builder.Buildings
{
    public class BuildingPlacer : MonoBehaviour
    {
        public Camera playerCamera;
        public LayerMask structuresPlacementGround;
        public LayerMask towerPlacementGround;
        public LayerMask placementBlocker;
        public LayerMask ui;

        BoxCollider prefabCollider;

        public TextMesh tooltipMesh;
        private Coroutine tooltip;


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

        private void Start()
        {
            tooltipMesh.transform.rotation = Camera.main.transform.rotation;
            tooltipMesh.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!FindFirstObjectByType<PlayerController>().isActiveAndEnabled)
            {
                CancelPlacing();
                return;
            }
            
            if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Escape) || Input.GetButton("Cancel")) // Right mouse button to cancel
            {
                CancelPlacing();
            }

            if (isPlacing)
            {
                HandlePlacement();
            }
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
            // Enable for controller
            // var input = new Vector3(Input.GetAxisRaw("HorizontalController"), Input.GetAxisRaw("VerticalController"),0);
            // Mouse.current.WarpCursorPosition(Input.mousePosition + input);

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, structuresPlacementGround) || Physics.Raycast(ray, out hit, Mathf.Infinity, towerPlacementGround))
            {
                prefabPreview.transform.position = hit.point;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementBlocker) || Physics.Raycast(ray, out hit, Mathf.Infinity, ui) || !IsPlacementValid())
                {
                    Debug.Log("Cannot Place here");
                    return;
                }
                else if (IsPlacementValid())
                {
                    Debug.Log("Can Place");
                }

                if (Input.GetMouseButtonDown(0) || Input.GetButton("Submit")) // Left mouse button
                {
                    if (IsPlacementValid())
                    {
                        if (ResourceTracker.Instance.SpendGold(currentPrefabCost) && !EventSystem.current.IsPointerOverGameObject())
                        {
                            PlaceBuilding();
                        }
                        else
                        {
                            tooltip = StartCoroutine(ShowToolTip("Not enough gold to place the building"));
                            Debug.Log("Not enough gold to place the building.");
                        }
                    }
                    else
                    {
                        tooltip = StartCoroutine(ShowToolTip("Invalid placement"));
                        Debug.Log("Invalid placement: The whole prefab is not on the ground.");
                    }
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

            if (currentPrefab.tag == "Tower")
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
            if (currentPrefab.tag == "Structure")
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
            if (prefabPreview != null)
            {
                Destroy(prefabPreview);
            }
            isPlacing = false;
        }

        private IEnumerator ShowToolTip(string text)
        {
            if(tooltip != null) StopCoroutine(tooltip);
            tooltipMesh.gameObject.SetActive(true);
            // Update the tooltip text with the current resources in the storage
            tooltipMesh.text = text;
            //     // Make the tooltip face the camera
            tooltipMesh.transform.rotation = Camera.main.transform.rotation;
            tooltipMesh.transform.position = prefabPreview.transform.position - new Vector3(0,0,-5);
            yield return new WaitForSeconds(2);
            tooltipMesh.gameObject.SetActive(false);
            tooltip = null;
        }
    }
}
