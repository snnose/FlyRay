using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeExplain : MonoBehaviour
{
    private static UpgradeExplain instance;

    public static UpgradeExplain Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }
    
    public GameObject icon;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeDetail;
    public TextMeshProUGUI priceText;
    public GameObject waffle;
    public GameObject purchase;

    private GameObject clickedUpgrade = null;
    
    private float upgradePrice = 0;    // 가격

    private void Awake()
    {
        upgradeName.text = "";
        upgradeDetail.text = "";
        priceText.text = "";
        waffle.SetActive(false);
        purchase.SetActive(false);
    }

    private void Start()
    {

    }

    public void OnClickUpgradeIcon()
    {
        waffle.SetActive(true);
        purchase.SetActive(true);

        clickedUpgrade =
            EventSystem.current.currentSelectedGameObject;
        icon.GetComponent<Image>().sprite = clickedUpgrade.GetComponent<Image>().sprite;

        UpgradeInfo info;

        string iconName = clickedUpgrade.name;

        switch (iconName)
        {
            case "ThrowUpgradeIcon":
                ThrowUpgrade throwUpgrade = clickedUpgrade.GetComponent<ThrowUpgrade>();
                info = throwUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.throwUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "AirDragUpgrade":
                AirDragUpgrade airdragUpgrade = clickedUpgrade.GetComponent<AirDragUpgrade>();
                info = airdragUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.airdragUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "WeightUpgrade":
                WeightUpgrade weightUpgrade = clickedUpgrade.GetComponent<WeightUpgrade>();
                info = weightUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.weightUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "FuelUpgradeIcon":
                FuelUpgrade fuelUpgrade = clickedUpgrade.GetComponent<FuelUpgrade>();
                info = fuelUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.fuelUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "MaroUpgrade":
                MaroUpgrade maroUpgrade = clickedUpgrade.GetComponent<MaroUpgrade>();
                info = maroUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.MaroUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "TrumpetUpgrade":
                TrumpetUpgrade trumpetUpgrade = clickedUpgrade.GetComponent<TrumpetUpgrade>();
                info = trumpetUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.trumpetUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "ChichuteUpgradeIcon":
                ChichuteUpgrade chichuteUpgrade = clickedUpgrade.GetComponent<ChichuteUpgrade>();
                info = chichuteUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.chichuteUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            case "BoosterUpgrade":
                BoosterUpgrade boosterUpgrade = clickedUpgrade.GetComponent<BoosterUpgrade>();
                info = boosterUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.boosterUpgrade == 1)
                    purchase.SetActive(false);
                else
                    purchase.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnClickUpgradePurchase()
    {
        if (clickedUpgrade != null)
        { 
            string upgradeName = clickedUpgrade.name;

            switch(upgradeName)
            {
                case "ThrowUpgradeIcon":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.throwUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "AirDragUpgrade":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.airdragUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "WeightUpgrade":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.weightUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "FuelUpgradeIcon":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.fuelUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "MaroUpgrade":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.MaroUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "TrumpetUpgrade":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.trumpetUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "ChichuteUpgradeIcon":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.chichuteUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;
                case "BoosterUpgrade":
                    if (DataManager.Instance.playerData.currentWaffle >= upgradePrice)
                    {
                        DataManager.Instance.playerData.boosterUpgrade = 1;
                        Purchase(clickedUpgrade);
                    }
                    break;

                default:
                    break;
            }

            DataManager.Instance.SaveData();
        }
    }

    // 투명도를 1로 변경
    private void SetAlpha1(GameObject gameObject)
    {
        Color col = gameObject.GetComponent<Image>().color;
        col.a = 1f;
        gameObject.GetComponent<Image>().color = col;
    }

    private void SetText(UpgradeInfo info)
    {
        upgradeName.text = info.GetName();
        upgradeDetail.text = info.GetDetail();
        priceText.text = info.GetPrice().ToString();
        upgradePrice = info.GetPrice();
    }

    private void Purchase(GameObject gameObject)
    {
        DataManager.Instance.SpendGold(upgradePrice);
        SetAlpha1(clickedUpgrade);
        purchase.SetActive(false);
    }

    public GameObject GetClickedUpgrade()
    {
        return this.clickedUpgrade;
    }
}
