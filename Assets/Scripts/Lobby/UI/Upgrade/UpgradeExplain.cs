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
    public TextMeshProUGUI price;
    public GameObject waffle;
    public GameObject purchase;

    private GameObject clickedUpgrade = null;

    private float p;

    private void Awake()
    {
        upgradeName.text = "";
        upgradeDetail.text = "";
        price.text = "";
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
            case "FuelUpgradeIcon":
                FuelUpgrade fuelUpgrade = clickedUpgrade.GetComponent<FuelUpgrade>();
                info = fuelUpgrade.GetInfo();
                SetText(info);

                if (DataManager.Instance.playerData.fuelUpgrade == 1)
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
                    DataManager.Instance.playerData.throwUpgrade = 1;
                    Purchase(clickedUpgrade);
                    break;
                case "FuelUpgradeIcon":
                    DataManager.Instance.playerData.fuelUpgrade = 1;
                    Purchase(clickedUpgrade);
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
        price.text = info.GetPrice().ToString();
        p = info.GetPrice();
    }

    private void Purchase(GameObject gameObject)
    {
        DataManager.Instance.SpendGold(p);
        SetAlpha1(clickedUpgrade);
        purchase.SetActive(false);
    }

    public GameObject GetClickedUpgrade()
    {
        return this.clickedUpgrade;
    }
}
