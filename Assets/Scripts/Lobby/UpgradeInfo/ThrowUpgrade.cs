using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowUpgradeInfo : UpgradeInfo
{
    public ThrowUpgradeInfo()
    {
        SetName("������ ��ȭ");
        SetDetail("������ ���� 50% �����մϴ�.");
        SetPrice(500f);
    }
}

public class ThrowUpgrade : MonoBehaviour
{
    private UpgradeInfo upgradeInfo = new ThrowUpgradeInfo();

    private void Start()
    {
        if (DataManager.Instance.playerData.throwUpgrade == 0)
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
