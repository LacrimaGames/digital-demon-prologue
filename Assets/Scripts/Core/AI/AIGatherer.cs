using System.Collections;
using System.Collections.Generic;
using DD.Builder.Buildings;
using DD.Environment;
using UnityEngine;
using UnityEngine.AI;

namespace DD.Core.AI
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class AIGatherer : MonoBehaviour
    {
        [Header("Global AI Modifers")]
        private int maxAmountHeld; // Maximum amount of resources the gatherer can carry
        private float gatheringSpeedPerSecond; // Time taken to gather one resource
        private int unloadAmountPerSecond; // Amount of materials unloaded per action
        private float unloadSpeedPerSecond; // Speed of unloading materials (units per second)

        [Header("Local AI Modifers")]
        public ResourceMaterial.Material materialToGather;
        public float gatherRange = 2f; // Range within which the gatherer can gather resources
        public float droppOffRange = 1f;
        private float unloadTimer = 0f; // Timer for unloading resources
        public float gatherTimer = 0f; // Timer for gathering resources
        public int amountHeld = 0; // Amount of resources currently carried

        [Header("AI Functions")]
        private List<GameObject> availableResource; // List of available trees from the Planter
        private GameObject currentResource; // The current tree being gathered
        private Storage nearbyStorage; // The nearby storage to deposit resources
        private Planter assignedPlanter; // Reference to the assigned Planter
        private Transform spawnPoint;
        private NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            if (GlobalModifiers.instance != null)
            {
                GlobalModifiers.FriendlyAIModifiers friendlyAIModifiers = GlobalModifiers.instance.LoadFriendlyAIModifiers();
                gatheringSpeedPerSecond = friendlyAIModifiers.gatheringSpeedPerSecond;
                maxAmountHeld = friendlyAIModifiers.maxAmountHeld;
                unloadSpeedPerSecond = friendlyAIModifiers.unloadSpeedPerSecond;
                unloadAmountPerSecond = friendlyAIModifiers.unloadAmount;
                navMeshAgent.speed = friendlyAIModifiers.movementSpeed;
            }
        }

        void Update()
        {

            if (assignedPlanter == null)
            {
                assignedPlanter = transform.root.GetComponent<Planter>();
            }

            if (materialToGather == ResourceMaterial.Material.None)
            {
                materialToGather = assignedPlanter.GetAvailableTrees()[0].GetComponent<ResourceMaterial>().resourceMaterial;
            }

            FindNearestStorage();

            if (nearbyStorage == null)
            {
                MoveTowards(spawnPoint.position);
                return;
            }

            gatherTimer -= Time.deltaTime;
            unloadTimer -= Time.deltaTime;
            availableResource = assignedPlanter.GetAvailableTrees();


            if (amountHeld >= maxAmountHeld)
            {
                StoreResources();
            }
            else if (nearbyStorage.currentCapacity < nearbyStorage.maxCapacity)
            {
                GatherResources();
            }
        }

        private void FindNearestStorage()
        {
            if (nearbyStorage == null || nearbyStorage.currentCapacity == nearbyStorage.maxCapacity)
            {
                foreach (var potentialStorage in FindObjectsOfType<Storage>())
                {
                    if (potentialStorage.storedMaterialType != materialToGather || potentialStorage.currentCapacity == potentialStorage.maxCapacity) continue;
                    nearbyStorage = potentialStorage;
                    break;
                }
            }
        }

        void GatherResources()
        {
            if (currentResource == null)
            {
                // Find the nearest available tree
                currentResource = FindNearestTree();
            }

            if (currentResource != null)
            {
                if (Vector3.Distance(transform.position, currentResource.transform.position) <= gatherRange)
                {
                    if (gatherTimer <= 0f)
                    {
                        Collider[] hitColliders = Physics.OverlapSphere(transform.position, gatherRange);
                        foreach (var hitCollider in hitColliders)
                        {
                            ResourceMaterial resource = hitCollider.GetComponent<ResourceMaterial>();
                            if (resource != null && amountHeld + 1 <= maxAmountHeld)
                            {
                                resource.Gather();
                                gatherTimer = gatheringSpeedPerSecond;
                                amountHeld++;
                                //break; // Only gather one resource at a time
                            }

                        }
                    }
                }
                else
                {
                    MoveTowards(currentResource.transform.position);
                }
            }
        }

        void StoreResources()
        {
            if (nearbyStorage != null)
            {

                if (Vector3.Distance(transform.position, nearbyStorage.transform.position) <= droppOffRange)
                {
                    navMeshAgent.isStopped = true;
                    StartCoroutine(UnloadResourcesCoroutine());
                }
                else
                {
                    MoveTowards(nearbyStorage.transform.position);
                }
            }
        }

        private IEnumerator UnloadResourcesCoroutine()
        {
            while (amountHeld > 0)
            {
                if (nearbyStorage == null) break;
                MoveTowards(nearbyStorage.transform.position);
                int amountToStore = Mathf.Min(amountHeld, unloadAmountPerSecond);

                // If carrying resources and there's a nearby storage, start unloading
                if (unloadTimer <= 0f && nearbyStorage != null)
                {
                    nearbyStorage.Unload(amountToStore);
                    amountHeld -= amountToStore;
                    unloadTimer = unloadSpeedPerSecond; // Reset the unload timer based on unload speed
                }
                yield return navMeshAgent.isStopped = false;
            }
        }

        void MoveTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            navMeshAgent.SetDestination(targetPosition);
            // transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }

        GameObject FindNearestTree()
        {
            GameObject nearestTree = null;
            float shortestDistance = Mathf.Infinity;

            foreach (GameObject tree in availableResource)
            {
                if (tree == null) continue;
                float distance = Vector3.Distance(transform.position, tree.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTree = tree;
                }
            }

            return nearestTree;
        }
    }
}
