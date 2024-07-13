using DD.Jobs;
using DD.Environment;
using UnityEngine;

namespace DD.Builder.Buildings
{
    public class Seller : MonoBehaviour
    {
        public float detectionRadius = 2f; // The radius within which the character can sell materials
        public MaterialValue[] materialValues; // Array of material values for different types
        public int totalMoney = 0;


        public void SellMaterials(ResourceMaterial.Material material, int amount)
        {
            if (material != ResourceMaterial.Material.None)
            {
                int materialAmount = amount;

                foreach (var materialValue in materialValues)
                {
                    if (materialValue.materialType == material)
                    {
                        int totalValue = materialAmount * materialValue.value;
                        totalMoney += totalValue;
                        //PlayerMoney.AddMoney(totalValue); // Add money to the player's total
                        break;
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

    [System.Serializable]
    public class MaterialValue
    {
        public ResourceMaterial.Material materialType; // The type of material
        public int value; // The value of the material
    }
}
