using System.Collections.Generic;
using DD.Builder.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DD.Core
{
    public class ButtonEnabler : MonoBehaviour
    {
        private List<GameObject> buttons = new List<GameObject>();
        private BuildingPlacer buildingPlacer;

        private LevelModifier levelModifier;

        // Start is called before the first frame update
        void Start()
        {
            buildingPlacer = GameObject.FindObjectOfType<BuildingPlacer>();
            levelModifier = LevelModifier.instance;
            foreach (Transform item in transform)
            {
                buttons.Add(item.gameObject);
                item.gameObject.SetActive(false);    
            }

            if(levelModifier.enabledBuildings.Count == 0)
            {
                GetComponent<Image>().enabled = false;
                return;
            }

            for (int i = 0; i < levelModifier.enabledBuildings.Count; i++)
            {
                buttons[i].SetActive(true);
                buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = levelModifier.enabledBuildings[i].ToString();
                buttons[i].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = buildingPlacer.buildings[i].cost.ToString();
            }
        }
    }
}
