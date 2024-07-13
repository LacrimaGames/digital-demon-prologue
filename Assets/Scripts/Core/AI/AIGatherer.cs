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
        public int maxCarriedResources = 10; // Maximum amount of resources the gatherer can carry
        public float gatherTime = 2f; // Time taken to gather one resource
        public float droppOffRange = 1f;

        public List<GameObject> availableTrees; // List of available trees from the Planter
        public int carriedResources = 0; // Amount of resources currently carried
        public GameObject currentTree; // The current tree being gathered
        public Storage nearbyStorage; // The nearby storage to deposit resources
        public Planter assignedPlanter; // Reference to the assigned Planter
        public float gatherTimer = 0f; // Timer for gathering resources

        NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            gatherTimer -= Time.deltaTime;
            if(nearbyStorage == null || nearbyStorage.currentCapacity == nearbyStorage.maxCapacity)
            {
                foreach (var potentialStorage in FindObjectsOfType<Storage>())
                {
                    if(potentialStorage.storedMaterialType != ResourceMaterial.Material.Wood || potentialStorage.currentCapacity == potentialStorage.maxCapacity) continue;
                    nearbyStorage = potentialStorage;
                    break;
                }
            }

            if (carriedResources >= maxCarriedResources)
            {
                StoreResources();
            }
            else if(nearbyStorage.currentCapacity < nearbyStorage.maxCapacity)
            {
                GatherResources();
            }
        }

        void GatherResources()
        {
            availableTrees = assignedPlanter.GetAvailableTrees();

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
                            if (resource != null && carriedResources + 1 <= maxCarriedResources)
                            {
                                resource.Gather();
                                gatherTimer = gatherTime;
                                carriedResources++;
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
                MoveTowards(nearbyStorage.transform.position);

                if (Vector3.Distance(transform.position, nearbyStorage.transform.position) <= droppOffRange)
                {
                    nearbyStorage.Unload(carriedResources);
                    carriedResources = 0; // Reset carried resources after storing
                    availableTrees = assignedPlanter.GetAvailableTrees();
                }
            }
        }

        void MoveTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
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
                if(tree == null) continue;
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
