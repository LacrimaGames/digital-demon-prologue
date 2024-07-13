using UnityEngine;

namespace DD.Core
{
    public class FollowPlayer : MonoBehaviour
    {
        public Transform player; // Reference to the player's transform
        public Vector3 offset; // Offset position from the player
        public bool followPlayer = true; // Toggle for following the player

        private Quaternion initialRotation; // Initial rotation to keep static

        void Start()
        {
            // Save the initial rotation of the object
            initialRotation = transform.rotation;
        }

        void Update()
        {
            if (followPlayer && player != null)
            {
                // Follow the player's position with the specified offset
                transform.position = player.position + offset;
                // Keep the rotation static
                transform.rotation = initialRotation;
            }
        }
    }
}
