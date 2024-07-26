using UnityEngine;

namespace DD.Core
{
    public class Tutorial : MonoBehaviour
    {

        public GameObject w;
        public GameObject a;
        public GameObject s;
        public GameObject d;


        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxis("Vertical") > 0)
            {
                w.SetActive(false);
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                s.SetActive(false);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                d.SetActive(false);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                a.SetActive(false);
            }
        }
    }
}


