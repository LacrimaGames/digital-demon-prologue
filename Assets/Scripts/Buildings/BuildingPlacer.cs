using System.Collections.Generic;
using UnityEngine;

namespace DD.Builder.Buildings
{
    public class BuildingPlacer : MonoBehaviour
    {
        public Camera playerCamera;
        public LayerMask groundLayer;
        public LayerMask placementBlocker;
        public GameObject[] buildingPrefabs;

        private GameObject currentPrefab;
        private GameObject prefabPreview;
        private bool isPlacing = false;

        void Update()
        {
            if (isPlacing)
            {
                HandlePlacement();
            }
            // Add more keys as needed for other buildings
        }

        public void StartPlacingBuilding(int prefabIndex)
        {
            if (prefabIndex >= 0 && prefabIndex < buildingPrefabs.Length)
            {
                if (prefabPreview != null)
                {
                    Destroy(prefabPreview);
                }

                currentPrefab = buildingPrefabs[prefabIndex];
                prefabPreview = Instantiate(currentPrefab.GetComponent<Builder>().previewPrefab);
                // prefabPreview.GetComponent<Collider>().enabled = false;
                isPlacing = true;
            }
        }

        void HandlePlacement()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                prefabPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0) && !Physics.Raycast(ray, out hit, Mathf.Infinity, placementBlocker)) // Left mouse button
                {
                    if (IsPlacementValid())
                    {
                        PlaceBuilding();
                    }
                    else
                    {
                        Debug.Log("Invalid placement: The whole prefab is not on the ground.");
                    }
                }
                else if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Escape) ) // Right mouse button to cancel
                {
                    CancelPlacing();
                }
            }
        }

        bool IsPlacementValid()
        {
            Collider prefabCollider = prefabPreview.GetComponent<Collider>();
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

            foreach (var point in checkPoints)
            {
                Ray ray = new Ray(point + Vector3.up * 10f, Vector3.down); // Cast ray downwards from above the point
                if (!Physics.Raycast(ray, 20f, groundLayer))
                {
                    return false;
                }
                if (Physics.Raycast(ray, 20f, placementBlocker))
                {
                    return false;
                }
            }

            return true;
        }

        void PlaceBuilding()
        {
            Instantiate(currentPrefab, prefabPreview.transform.position, prefabPreview.transform.rotation);
            Destroy(prefabPreview);
            isPlacing = false;
        }

        void CancelPlacing()
        {
            Destroy(prefabPreview);
            isPlacing = false;
        }
    }
}
