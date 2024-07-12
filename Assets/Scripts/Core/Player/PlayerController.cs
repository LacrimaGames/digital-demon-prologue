using UnityEngine;

namespace DD.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // Speed of the player movement
        public float rotationSpeed = 720f; // Speed of the player rotation (degrees per second)

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Get input from the horizontal and vertical axes
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Create a vector for the movement
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            // Move the player by setting the velocity of the Rigidbody
            rb.velocity = movement * moveSpeed;

            // If there is movement input, rotate the player towards the movement direction
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
