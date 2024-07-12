
using UnityEngine;
namespace DD.Environment
{
    public class ResourceMaterial : MonoBehaviour
    {
        public enum Material
        {
            None,
            Wood,
            Stone,
            Metal,
            Water,
            // Add more material types as needed
        }
        public int health = 5;
        public Material resourceMaterial = Material.None;

        public void Gather()
        {
            health--;

            if (health <= 0)
            {
                Destroy(gameObject); // Destroy the tree when health reaches 0
            }
        }
    }
}



