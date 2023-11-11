using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUpgradeInfo : UpgradeInfo
{
    public FuelUpgradeInfo()
    {
        SetName("��� ����");
        SetDetail("���� ȿ���� 50% �����մϴ�.");
        SetPrice(300f);
    }
}



public class FuelUpgrade : MonoBehaviour
{
    private UpgradeInfo upgradeInfo = new FuelUpgradeInfo();

    private void Start()
    {
        if (DataManager.Instance.playerData.fuelUpgrade == 0)
        {
            Color col = this.GetComponent<Image>().color;
            col.a = 0.5f;
            this.GetComponent<Image>().color = col;
        }
    }

    public UpgradeInfo GetInfo()
    {
        return this.upgradeInfo;
    }
}
