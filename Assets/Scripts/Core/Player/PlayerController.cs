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
        public GameObject bow;

        private void Start()
        {
            EnableTool();
        }


        void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                EnableTool();
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                EnableWeapon();
            }
        }

        private void EnableWeapon()
        {
            hasWeaponEquipped = true;
            hasToolEquipped = !hasWeaponEquipped;
            GetComponent<Animation>().clip = shooting;
            axe.SetActive(false);
            bow.SetActive(true);
        }

        private void EnableTool()
        {
            hasToolEquipped = true;
            hasWeaponEquipped = !hasToolEquipped;
            GetComponent<Animation>().clip = axeSwinging;
            axe.SetActive(true);
            bow.SetActive(false);
        }
    }
}
