using UnityEngine;
using DD.Builder.Buildings;

namespace DD.Builder
{
    public class Builder : MonoBehaviour
    {
        public Storage woodStorage; // The storage object that holds wood
        public int woodNeeded; // The amount of wood needed for the house

        public bool needsStone; // Toggle option for requiring stone
        public Storage stoneStorage; // The storage object that holds stone (if needed)
        public int stoneNeeded; // The amount of stone needed for the house

        public GameObject floor; // The floor GameObject
        public GameObject walls; // The walls GameObject
        public GameObject roof; // The roof GameObject
        public GameObject constructionFence; // The fence GameObject


        public GameObject resource; // The resource GameObject

        public bool unlocksFunction = false;

        private int woodGathered = 0;
        private int stoneGathered = 0;

        public GameObject previewPrefab;

        private void Awake() // If object has other scripts, disable them until the building is finished
        {
            foreach (var components in GetComponents<MonoBehaviour>())
            {
                if (components == this) continue;
                unlocksFunction = true;
            }
        }

        private void Start()
        {
            if (woodStorage != null)
            {
                woodStorage.maxCapacity = woodNeeded;
            }

            if (needsStone && stoneStorage != null)
            {
                stoneStorage.maxCapacity = stoneNeeded;
            }
        }

        public void DisableBuilding()
        {
            floor.SetActive(false);
            walls.SetActive(false);
            roof.SetActive(false);
        }

        void Update()
        {
            if (woodStorage != null)
            {
                woodGathered = woodStorage.GetCurrentCapacity();
            }

            if (needsStone && stoneStorage != null)
            {
                stoneGathered = stoneStorage.GetCurrentCapacity();
            }

            UpdateBuildingProgress();

            if (IsBuildingComplete())
            {
                UnlockResource();
                UnlockFunction();
                CleanUp();
            }
        }

        void UpdateBuildingProgress()
        {
            float totalRequired = woodNeeded + (needsStone ? stoneNeeded : 0);
            float totalGathered = woodGathered + stoneGathered;

            if (totalGathered >= totalRequired / 3)
            {
                floor.SetActive(true);
            }

            if (totalGathered >= 2 * totalRequired / 3)
            {
                walls.SetActive(true);
            }

            if (totalGathered >= totalRequired)
            {
                roof.SetActive(true);
            }

            if (woodStorage != null && woodNeeded == woodGathered)
            {
                Destroy(woodStorage.gameObject);
            }

            if (needsStone && stoneStorage != null && stoneNeeded == stoneGathered)
            {
                Destroy(stoneStorage.gameObject);
            }
        }

        bool IsBuildingComplete()
        {
            return woodGathered >= woodNeeded && (!needsStone || stoneGathered >= stoneNeeded);
        }

        void UnlockResource()
        {
            if (resource != null)
            {
                resource.SetActive(true);
            }
        }

        void UnlockFunction()
        {
            if (unlocksFunction)
            {
                foreach (var function in GetComponents<MonoBehaviour>())
                {
                    function.enabled = true;
                }
            }
        }

        void CleanUp()
        {
            Destroy(constructionFence);
            Destroy(GetComponent<Builder>());
        }
    }
}
