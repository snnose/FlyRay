using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeControl : MonoBehaviour
{ 
    // 싱글톤
    private static UpgradeControl instance;
    public static UpgradeControl Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }

    private GameObject upgradeUI;
    public GameObject purchase;

    public GameObject throwUpgradeIcon;
    public GameObject fuelUpgradeIcon;
    private void Awake()
    {
        upgradeUI = GameObject.FindGameObjectWithTag("UpgradeUI");

        upgradeUI.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        upgradeUI.SetActive(false);
    }

    public void OnClickUpgrade()
    {
        upgradeUI.SetActive(true);
    }

    public void OnClickUpgradeX()
    {
        upgradeUI.SetActive(false);
    }

    public void OnClickUpgradeInit()
    {
        // 업그레이드 내역을 초기화하고 골드를 돌려 받는다.
        DataManager.Instance.InitUpgrade();
        // 아이콘 투명도 0.5로 설정
        SetAlphaHalf(throwUpgradeIcon);
        SetAlphaHalf(fuelUpgradeIcon);
        // 구매 버튼이 비활성화 상태이면 활성화
        if (!purchase.activeSelf)
            purchase.SetActive(true);
    }

    private void SetAlphaHalf(GameObject gameObject)
    {
        Color col = gameObject.GetComponent<Image>().color;
        col.a = 0.5f;
        gameObject.GetComponent<Image>().color = col;
    }
}
