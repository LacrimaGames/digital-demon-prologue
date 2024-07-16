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

    private TMP_Text materialText;
    private TMP_Text goldText;
    private TMP_Text amountText;

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

    private void Start() {
        if (materialText == null)
        {
            materialText = GameObject.FindGameObjectWithTag("MaterialType").GetComponent<TMP_Text>();
        }

        if (goldText == null)
        {
            goldText = GameObject.FindGameObjectWithTag("GoldAmount").GetComponent<TMP_Text>();
        }

        if (amountText == null)
        {
            amountText = GameObject.FindGameObjectWithTag("MaterialAmount").GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<PlayerGatherer>() != null)
        {
            currentMaterialHeld = FindAnyObjectByType<PlayerGatherer>().typeOfMaterialHeld;
            amountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().amountHeld;
            maxAmountOfMaterialHeld = FindAnyObjectByType<PlayerGatherer>().maxAmountHeld;
        }

        goldText.text = amountOfGold.ToString();
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
