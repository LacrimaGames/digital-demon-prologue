using UnityEngine;

namespace DD.Environment
{
    public class Planter : MonoBehaviour
    {
        public GameObject treePrefab; // The tree prefab to spawn
        public Transform[] treePlots; // Array of transforms representing the tree plots
        public float checkInterval = 5f; // Interval (in seconds) to check for empty plots

        private float checkTimer;

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
            foreach (var plot in treePlots)
            {
                if (plot.childCount == 0)
                {
                    Instantiate(treePrefab, plot.position, plot.rotation, plot);
                }
            }
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
