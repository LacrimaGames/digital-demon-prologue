using UnityEngine;

namespace DD.Builder.Buildings
{
    public class PlacementBlocker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Preview")
            {
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


