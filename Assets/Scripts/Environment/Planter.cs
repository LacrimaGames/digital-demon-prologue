using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class Planter : MonoBehaviour
    {
        public GameObject treePrefab; // The tree prefab to spawn
        public Transform[] treePlots; // Array of transforms representing the tree plots
        public float checkInterval = 5f; // Interval (in seconds) to check for empty plots
        private List<GameObject> availableTrees = new List<GameObject>();

        private float checkTimer;

        public Transform spawnpointAIGatherer;

        void Start()
        {
            checkTimer = checkInterval;
            RespawnTrees();
        }

        void Update()
        {
            checkTimer -= Time.deltaTime;

            if (checkTimer <= 0f)
            {
                RespawnTrees();
                checkTimer = checkInterval;
            }
        }

        void RespawnTrees()
        {
            availableTrees = new List<GameObject>();

            foreach (var plot in treePlots)
            {
                if (plot.childCount == 0)
                {
                    GameObject tree = Instantiate(treePrefab, plot.position, plot.rotation, plot);
                    availableTrees.Add(tree);
                }
                else
                {
                    availableTrees.Add(plot.GetChild(0).gameObject);
                }
            }


        }
        
        public List<GameObject> GetAvailableTrees()
        {
            return availableTrees;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var plot in treePlots)
            {
                Gizmos.DrawWireCube(plot.position, new Vector3(1, 1, 1));
            }
        }
    }
}
