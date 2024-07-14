using System.Collections;
using System.Collections.Generic;
using DD.Builder.Buildings;
using DD.Environment;
using UnityEngine;
using UnityEngine.AI;

namespace DD.Core.AI
{
    public class AIGatherer : MonoBehaviour
    {
        public float gatherRange = 2f; // Range within which the gatherer can gather resources
        public int maxAmountHeld = 10; // Maximum amount of resources the gatherer can carry
        public float gatherTime = 2f; // Time taken to gather one resource
        public float droppOffRange = 1f;
        public int unloadAmount = 1; // Amount of materials unloaded per action
        public float unloadSpeed = 1; // Speed of unloading materials (units per second)

        private float unloadTimer;

        public List<GameObject> availableTrees; // List of available trees from the Planter
        public int amountHeld = 0; // Amount of resources currently carried
        public GameObject currentTree; // The current tree being gathered
        public Storage nearbyStorage; // The nearby storage to deposit resources
        public Planter assignedPlanter; // Reference to the assigned Planter
        public float gatherTimer = 0f; // Timer for gathering resources

        public Transform spawnPoint;

        NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            spawnPoint = assignedPlanter.spawnpointAIGatherer;
        }

        void Update()
        {
            FindNearestStorage();

            if (nearbyStorage == null) 
            {
                MoveTowards(spawnPoint.position);
                return;
            }

            gatherTimer -= Time.deltaTime;
            unloadTimer -= Time.deltaTime;
            availableTrees = assignedPlanter.GetAvailableTrees();


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
                    if (potentialStorage.storedMaterialType != ResourceMaterial.Material.Wood || potentialStorage.currentCapacity == potentialStorage.maxCapacity) continue;
                    nearbyStorage = potentialStorage;
                    break;
                }
            }
        }

        void GatherResources()
        {
            if (currentTree == null)
            {
                // Find the nearest available tree
                currentTree = FindNearestTree();
            }

            if (currentTree != null)
            {
                if (Vector3.Distance(transform.position, currentTree.transform.position) <= gatherRange)
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
                                gatherTimer = gatherTime;
                                amountHeld++;
                                //break; // Only gather one resource at a time
                            }

                        }
                    }
                }
                else
                {
                    MoveTowards(currentTree.transform.position);
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
                if(nearbyStorage == null) break;
                MoveTowards(nearbyStorage.transform.position);
                int amountToStore = Mathf.Min(amountHeld, unloadAmount);

                // If carrying resources and there's a nearby storage, start unloading
                if (unloadTimer <= 0f && nearbyStorage != null)
                {
                    nearbyStorage.Unload(amountToStore);
                    amountHeld -= amountToStore;
                    unloadTimer = unloadSpeed; // Reset the unload timer based on unload speed
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

            foreach (GameObject tree in availableTrees)
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
