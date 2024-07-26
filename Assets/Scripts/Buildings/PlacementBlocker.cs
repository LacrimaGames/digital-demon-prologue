using UnityEngine;

namespace DD.Builder.Buildings
{
    public class PlacementBlocker : MonoBehaviour
    {

        private GameObject colliderObject;

        private void Update()
        {
            if(colliderObject == null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Preview")
            {
                colliderObject = other.gameObject;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Preview")
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Preview")
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}


