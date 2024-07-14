using DD.Builder.Buildings;
using DD.Environment;
using DD.Jobs;
using TMPro;
using UnityEngine;

public class ResourceTracker : MonoBehaviour
{

    // This is Pseudocode

    private ResourceMaterial.Material currentMaterialHeld;
    private int amountOfMaterialHeld;
    private int maxAmountOfMaterialHeld;

    private int amountOfGold;


    public TMP_Text materialText;
    public TMP_Text goldText;
    public TMP_Text amountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<Seller>() != null)
        {
            amountOfGold = FindObjectOfType<Seller>().totalMoney;
            goldText.text = amountOfGold.ToString();
        }

        currentMaterialHeld = FindAnyObjectByType<PlayerGatherer>().typeOfMaterialHeld;
        amountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().amountHeld;
        maxAmountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().maxAmountHeld;

        amountText.text = amountOfMaterialHeld.ToString() + " / " + maxAmountOfMaterialHeld;

        if(currentMaterialHeld == ResourceMaterial.Material.Wood)
        {
            materialText.text = "Wood";
        }
        else if (currentMaterialHeld == ResourceMaterial.Material.Stone)
        {
            materialText.text = "Stone";
        }
        else
        {
            materialText.text = "None";
        }
    }
}
