using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DD.UI
{
    public class UIWrapper3D : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

}

