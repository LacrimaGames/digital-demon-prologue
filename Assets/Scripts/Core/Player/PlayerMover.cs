using UnityEngine;

namespace DD.Core.Player
{
    public class PlayerMover : MonoBehaviour
    {
        public float speed = 5f; // Speed of the player
        public float rotationSpeed = 700f; // Speed of rotation
        public Camera mainCamera; // Reference to the main camera

        void Update()
        {
            Move();
        }

        void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Get the camera's forward and right vectors
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            // Flatten the camera's forward and right vectors on the XZ plane
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate the direction to move based on camera orientation
            Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                // Calculate target angle and rotate the player smoothly
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                // Move the player in the direction of the camera's forward axis
                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            }
        }
    }
}
