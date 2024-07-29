using UnityEngine;
using UnityEngine.EventSystems;

namespace DD.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool hasWeaponEquipped = false; // Toggle to check if the player has a weapon equipped
        public bool hasToolEquipped = true; // Toggle to check if the player has a tool equipped

        public AnimationClip axeSwinging;
        public AnimationClip shooting;

        public GameObject axe;

        void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                hasWeaponEquipped = true;
                hasToolEquipped = !hasWeaponEquipped;
                GetComponent<Animation>().clip = shooting;
                axe.SetActive(false);
            }

            if (Input.GetKey(KeyCode.Alpha2) )
            {
                hasToolEquipped = true;
                hasWeaponEquipped = !hasToolEquipped;
                GetComponent<Animation>().clip = axeSwinging;
                axe.SetActive(true);
            }
        }
    }
}
