using UnityEngine;

namespace DD.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool hasWeaponEquipped = false; // Toggle to check if the player has a weapon equipped
        public bool hasToolEquipped = true; // Toggle to check if the player has a tool equipped

        void Update()
        {
            // Enable for controller
            // if (Input.GetButtonDown("Switch"))
            // {
            //     if(hasWeaponEquipped)
            //     {
            //         hasWeaponEquipped = false;
            //         hasToolEquipped = true;
            //     }
            //     else if(hasToolEquipped)
            //     {
            //         hasToolEquipped = false;
            //         hasWeaponEquipped = true;
            //     }
            // }

            if (Input.GetKey(KeyCode.Alpha1))
            {
                hasWeaponEquipped = true;
                hasToolEquipped = !hasWeaponEquipped;
            }

            if (Input.GetKey(KeyCode.Alpha2) )
            {
                hasToolEquipped = true;
                hasWeaponEquipped = !hasToolEquipped;
            }
        }
    }
}
