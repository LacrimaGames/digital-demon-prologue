using System;
using System.Collections.Generic;
using DD.Core;
using DD.Environment;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace DD.Jobs
{
    public class Backpack : MonoBehaviour
    {
        private PlayerGatherer playerGatherer;
        public float maxAmountHeld;
        public float currentCapacity;

        public GameObject logs;
        public GameObject stones;

        private List<GameObject> woodLogs = new List<GameObject>();
        private List<GameObject> stoneBoulders = new List<GameObject>();


        public ResourceMaterial.Material materialHeld;
        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in logs.transform)
            {
                woodLogs.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in stones.transform)
            {
                stoneBoulders.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }

            playerGatherer = transform.parent.GetComponent<PlayerGatherer>();

            if (GlobalModifiers.instance != null)
            {
                GlobalModifiers.PlayerModifiers playerModifiers = GlobalModifiers.instance.LoadPlayerModifiers();
                maxAmountHeld = playerModifiers.maxAmountHeld;
            }
        }

        // Update is called once per frame
        void Update()
        {
            materialHeld = playerGatherer.typeOfMaterialHeld;
            currentCapacity = playerGatherer.amountHeld;

            CalculatePercentage(materialHeld);
        }

        private void CalculatePercentage(ResourceMaterial.Material materialHeld)
        {
            if (materialHeld == ResourceMaterial.Material.Wood)
            {
                DisplayResources(ResourceMaterial.Material.Wood, Mathf.RoundToInt(woodLogs.Count / maxAmountHeld * currentCapacity));
            }
            else if (materialHeld == ResourceMaterial.Material.Stone)
            {
                DisplayResources(ResourceMaterial.Material.Stone, Mathf.RoundToInt(stoneBoulders.Count / maxAmountHeld * currentCapacity));
            }
            else if (materialHeld == ResourceMaterial.Material.None)
            {
                //
            }
        }

        void DisplayResources(ResourceMaterial.Material material, int amount)
        {
            if (material == ResourceMaterial.Material.Wood)
            {
                for (int i = 0; i < amount; i++)
                {
                    woodLogs[i].SetActive(true);
                }
                for (int i = amount; i < woodLogs.Count; i++)
                {
                    woodLogs[i].SetActive(false);
                }
            }
            else if (materialHeld == ResourceMaterial.Material.Stone)
            {
                for (int i = 0; i < amount; i++)
                {
                    stoneBoulders[i].SetActive(true);
                }
                for (int i = amount; i < stoneBoulders.Count; i++)
                {
                    stoneBoulders[i].SetActive(false);
                }
            }
        }
    }

}

