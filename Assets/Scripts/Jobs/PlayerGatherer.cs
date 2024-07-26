using DD.Builder.Buildings;
using DD.Core;
using DD.Core.Player;
using DD.Environment;
using UnityEngine;

namespace DD.Jobs
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerGatherer : MonoBehaviour
    {

        [Header("Global Player Modifers")]
        private float gatheringSpeedPerSecond; // Time between each gathering action
        private float unloadSpeedPerSecond; // Speed of unloading materials (units per second)
        private int maxAmountHeld;

        [Header("Local Player Modifers")]
        public int amountHeld = 0;
        public float detectionRadius = 3f; // The radius within which the player can detect and gather materials
        public float unloadRadius = 2f; // The radius within which the character can unload materials
        private int unloadAmountPerSecond = 1; // Amount of materials unloaded per action
        public float sellSpeed = 1; // Speed of unloading materials (units per second)
        public int sellAmount = 1;
        public ResourceMaterial.Material typeOfMaterialHeld = ResourceMaterial.Material.None;

        private float gatherTimer;
        private float unloadTimer;
        private float sellTimer;

        private PlayerController playerController;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            if (GlobalModifiers.instance != null)
            {
                GlobalModifiers.PlayerModifiers playerModifiers = GlobalModifiers.instance.LoadPlayerModifiers();
                gatheringSpeedPerSecond = playerModifiers.gatheringSpeedPerSecond;
                maxAmountHeld = playerModifiers.maxAmountHeld;
                unloadSpeedPerSecond = playerModifiers.unloadSpeedPerSecond;
            }
        }

        void Update()
        {
            gatherTimer -= Time.deltaTime;
            unloadTimer -= Time.deltaTime;
            sellTimer -= Time.deltaTime;

            Build();
            Gather();
            Sell();
        }

        private void Sell()
        {
            if (sellTimer <= 0f)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
                foreach (var hitCollider in hitColliders)
                {
                    Seller seller = hitCollider.GetComponent<Seller>();
                    if (seller != null)
                    {
                        int ammountToSell = Mathf.Min(amountHeld, sellAmount);
                        seller.SellMaterials(typeOfMaterialHeld, ammountToSell);

                        amountHeld -= ammountToSell;

                        if (amountHeld == 0)
                        {
                            typeOfMaterialHeld = ResourceMaterial.Material.None;
                        }

                        sellTimer = sellSpeed; // Reset the unload timer based on unload speed
                        break; // Only unload to one storage at a time
                    }
                }
            }

        }

        private void Build()
        {
            if (unloadTimer <= 0f)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, unloadRadius);
                foreach (var hitCollider in hitColliders)
                {
                    Storage storage = hitCollider.GetComponent<Storage>();
                    int amountToStore = Mathf.Min(amountHeld, unloadAmountPerSecond);

                    if (storage != null && CanUnload(storage, amountToStore))
                    {
                        storage.Unload(amountToStore);
                        amountHeld -= amountToStore;

                        if (amountHeld == 0)
                        {
                            typeOfMaterialHeld = ResourceMaterial.Material.None;
                        }

                        unloadTimer = unloadSpeedPerSecond; // Reset the unload timer based on unload speed
                        break; // Only unload to one storage at a time
                    }
                }
            }
        }

        private void Gather()
        {
            if (gatherTimer <= 0f && playerController.hasToolEquipped)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
                foreach (var hitCollider in hitColliders)
                {
                    ResourceMaterial resource = hitCollider.GetComponent<ResourceMaterial>();
                    if (resource != null && CanGather(resource.resourceMaterial))
                    {
                        resource.Gather();
                        gatherTimer = gatheringSpeedPerSecond;
                        amountHeld++;
                        typeOfMaterialHeld = resource.resourceMaterial;
                        //break; // Only gather one resource at a time
                    }

                }
            }
        }

        bool CanGather(ResourceMaterial.Material materialType)
        {
            if (typeOfMaterialHeld != ResourceMaterial.Material.None && typeOfMaterialHeld != materialType)
            {
                return false; // Prevent gathering if holding a different type of material
            }
            if (amountHeld >= maxAmountHeld)
            {
                return false; // Prevent gathering if at max capacity
            }
            return true;
        }

        bool CanUnload(Storage storage, int unloadAmount)
        {
            if (storage.storedMaterialType == typeOfMaterialHeld && amountHeld > 0 && storage.currentCapacity + unloadAmount <= storage.maxCapacity)
            {
                return true;
            }
            return false;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
