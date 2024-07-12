using UnityEngine;
using DD.Environment;

namespace DD.Builder.Buildings
{
    public class Storage : MonoBehaviour
    {
        public ResourceMaterial.Material storedMaterialType; // The type of material this storage can hold
        public int maxCapacity = 100; // Maximum capacity of the storage
        public float unloadSpeed = 1f; // Speed of unloading materials (units per second)
        public int currentCapacity = 0; // Current amount of material in storage

        public void Unload(int amount)
        {
            currentCapacity += amount;
        }

        public int GetCurrentCapacity()
        {
            return currentCapacity;
        }

        public int GetAvailableSpace()
        {
            return maxCapacity - currentCapacity;
        }
    }
}
