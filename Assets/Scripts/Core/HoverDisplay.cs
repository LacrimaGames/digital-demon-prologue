using DD.Builder.Buildings;
using UnityEngine;

namespace DD.Core
{
    public class HoverDisplay : MonoBehaviour
    {
        private Storage storage; // Reference to the storage component
        public TextMesh tooltipTextMesh; // Reference to the TextMesh component for displaying resources
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            storage = GetComponent<Storage>();
        }

        private void Update()
        {
            // // Check if the player is hovering over the storage
            // if (IsPlayerHovering())
            // {
            //     // Update and display the tooltip
            //     UpdateTooltip();
            //     tooltipTextMesh.gameObject.SetActive(true);

            //     // Position the tooltip above the storage
            //     tooltipTextMesh.transform.position = transform.position + Vector3.up;
            //     // Make the tooltip face the camera
            //     tooltipTextMesh.transform.rotation = mainCamera.transform.rotation;
            // }
            // else
            // {
            //     tooltipTextMesh.gameObject.SetActive(false); // Hide the tooltip
            // }

            UpdateTooltip();

        }

        // private bool IsPlayerHovering()
        // {
        //     // Perform a raycast to check if the player is hovering over the storage
        //     Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         return hit.transform == transform;
        //     }
        //     return false;
        // }

        private void UpdateTooltip()
        {
            // Update the tooltip text with the current resources in the storage
            tooltipTextMesh.text = $"{storage.currentCapacity} / {storage.maxCapacity} {storage.storedMaterialType}";
            //     // Make the tooltip face the camera
                tooltipTextMesh.transform.rotation = mainCamera.transform.rotation;
        }
    }
}
