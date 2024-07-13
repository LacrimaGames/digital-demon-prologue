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

        private int woodGathered = 0;
        private int stoneGathered = 0;

        private void Start() {
            if (woodStorage != null)
            {
                woodStorage.maxCapacity = woodNeeded;
            }

            if (needsStone && stoneStorage != null)
            {
                stoneStorage.maxCapacity = stoneNeeded;
            }
        }

        void Update()
        {
            if (woodStorage != null)
            {
                // int woodAvailable = woodStorage.GetCurrentCapacity();
                // int woodToGather = Mathf.Min(woodAvailable, woodNeeded - woodGathered);

                // woodStorage.Unload(woodToGather);
                // woodGathered += woodToGather;
                woodGathered = woodStorage.GetCurrentCapacity();
            }

            if (needsStone && stoneStorage != null)
            {
                // int stoneAvailable = stoneStorage.GetCurrentCapacity();
                // int stoneToGather = Mathf.Min(stoneAvailable, stoneNeeded - stoneGathered);

                // stoneStorage.Unload(stoneToGather);
                // stoneGathered += stoneToGather;
                stoneGathered = stoneStorage.GetCurrentCapacity();
            }

            UpdateBuildingProgress();

            if (IsBuildingComplete())
            {
                DestroyStorages();
                Destroy(GetComponent<Builder>());
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
        }

        bool IsBuildingComplete()
        {
            return woodGathered >= woodNeeded && (!needsStone || stoneGathered >= stoneNeeded);
        }

        void DestroyStorages()
        {
            if (woodStorage != null)
            {
                Destroy(woodStorage.gameObject);
            }

            if (needsStone && stoneStorage != null)
            {
                Destroy(stoneStorage.gameObject);
            }
        }
    }
}
