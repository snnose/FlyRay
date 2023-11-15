using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeControl : MonoBehaviour
{ 
    // �̱���
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
        // ���׷��̵� ������ �ʱ�ȭ�ϰ� ��带 ���� �޴´�.
        DataManager.Instance.InitUpgrade();
        // ������ ���� 0.5�� ����
        SetAlphaHalf(throwUpgradeIcon);
        SetAlphaHalf(fuelUpgradeIcon);
        // ���� ��ư�� ��Ȱ��ȭ �����̸� Ȱ��ȭ
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
