using DD.Core.Player;
using UnityEngine;

namespace DD.Core
{
    public class Hotbar : MonoBehaviour
    {

        public GameObject toolUI;
        public GameObject weaponUI;

        PlayerController playerController;

        private void Start() {
            if(playerController == null)
            {
                playerController = FindObjectOfType<PlayerController>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(playerController.hasToolEquipped)
            {
                toolUI.SetActive(true);
                weaponUI.SetActive(false);
            }

            if (playerController.hasWeaponEquipped)
            {
                weaponUI.SetActive(true);
                toolUI.SetActive(false);
            }
        }
    }
}


