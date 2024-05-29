using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Upgrade")]
    public Button upgradeButton;
    public TextMeshProUGUI upgradeCostText;
    public float upgradeBaseCost = 10000;
    public GameObject[] upgradeList;
    private int upgradeIndex = 0;

    [Header("Auto Clicker")]
    public Button autoClickerButton;
    public TextMeshProUGUI autoClickerCostText;
    public float autoClickerBaseCost = 500;
    public Transform autoClickerParent;
    public GameObject autoClickerPrefab;
    private int autoClickerCount = 0;

    [Header("Chefs")]
    public Button chefButton;
    public TextMeshProUGUI chefCostText;
    public TextMeshProUGUI chefCountText;
    public float chefBaseCost = 1000;
    public int chefBaseCps = 1;
    private int chefCount = 0;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Load();

        upgradeButton.onClick.AddListener(BuyUpgrade);
        autoClickerButton.onClick.AddListener(BuyAutoClicker);
        chefButton.onClick.AddListener(BuyChef);

        upgradeCostText.text = $"Price: {(long)upgradeBaseCost}";
        autoClickerCostText.text = $"Price: {(long)autoClickerBaseCost}";
        chefCostText.text = $"Price: {(long)chefBaseCost}";

        InvokeRepeating("ChefBake", 1, 1);
    }


    void ChefBake()
    {
        if (chefCount > 0)
        {
            Clicker.instance.Clicks += chefCount * chefBaseCps;
        }
    }


    private void Update()
    {
        if (Clicker.instance.Clicks >= (long)upgradeBaseCost)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }

        if (Clicker.instance.Clicks >= (long)autoClickerBaseCost)
        {
            autoClickerButton.interactable = true;
        }
        else
        {
            autoClickerButton.interactable = false;
        }

        if (Clicker.instance.Clicks >= (long)chefBaseCost)
        {
            chefButton.interactable = true;
        }
        else
        {
            chefButton.interactable = false;
        }
    }


    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            Save();
        }   
    }


    private void OnApplicationQuit()
    {
        Save();
    }


    void BuyUpgrade()
    {
        audioSource.Play();
        Clicker.instance.Clicks -= (long)upgradeBaseCost;
        upgradeBaseCost *= 2f;
        upgradeCostText.text = $"Price: {(long)upgradeBaseCost}";

        upgradeList[upgradeIndex].SetActive(false);
        upgradeIndex++;
        Clicker.clickValue++;
        upgradeList[upgradeIndex].SetActive(true);

        if (upgradeIndex == upgradeList.Length - 1)
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }


    void BuyAutoClicker()
    {
        audioSource.Play();
        Clicker.instance.Clicks -= (long)autoClickerBaseCost;
        autoClickerBaseCost *= 1.5f;
        autoClickerCostText.text = $"Price: {(long)autoClickerBaseCost}";

        //spawn helper
        SpawnAutoClicker();
    }


    void SpawnAutoClicker()
    {
        autoClickerCount++;
        var pos = new Vector3();
        var angle = 36 * autoClickerCount;
        pos.x = Mathf.Cos(Mathf.Deg2Rad * angle) * 0.17f;
        pos.y = Mathf.Sin(Mathf.Deg2Rad * angle) * 0.17f;
        var pointer = Instantiate(autoClickerPrefab, autoClickerParent.position, Quaternion.identity);
        pointer.transform.SetParent(autoClickerParent);
        pointer.transform.localPosition = pos;
        pointer.transform.LookAt(autoClickerParent.position);

        if (autoClickerCount == 10)
        {
            autoClickerButton.gameObject.SetActive(false);
        }
    }


    void BuyChef()
    {
        audioSource.Play();
        Clicker.instance.Clicks -= (long)chefBaseCost;
        chefBaseCost *= 1.5f;
        chefCostText.text = $"Price: {(long)chefBaseCost}";
        chefCount++;
        chefCountText.text = chefCount.ToString();
        chefCount++;
    }


    public void Load()
    {
        chefCount = PlayerPrefs.GetInt("chefCount", 0);
        chefCountText.text = chefCount.ToString();

        var count = PlayerPrefs.GetInt("autoClickerCount", 0);
        for (int i = 0; i < count; i++)
        {
            SpawnAutoClicker();
        }

        upgradeList[upgradeIndex].SetActive(false);
        upgradeIndex = PlayerPrefs.GetInt("upgradeIndex", 0);
        upgradeList[upgradeIndex].SetActive(true);
    }


    public void Save()
    {
        PlayerPrefs.SetInt("chefCount", chefCount);
        PlayerPrefs.SetInt("autoClickerCount", autoClickerCount);
        PlayerPrefs.SetInt("upgradeIndex", upgradeIndex);
    }
}
