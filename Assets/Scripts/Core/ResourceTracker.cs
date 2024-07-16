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

    public static ResourceTracker Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = amountOfGold.ToString();

        currentMaterialHeld = FindAnyObjectByType<PlayerGatherer>().typeOfMaterialHeld;
        amountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().amountHeld;
        maxAmountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().maxAmountHeld;

        amountText.text = amountOfMaterialHeld.ToString() + " / " + maxAmountOfMaterialHeld;

        if (currentMaterialHeld == ResourceMaterial.Material.Wood)
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

    public void AddGold(int amount)
    {
        amountOfGold += amount;
    }

    public bool SpendGold(int amount)
    {
        if (amountOfGold >= amount)
        {
            amountOfGold -= amount;
            return true;
        }
        return false;
    }


}
