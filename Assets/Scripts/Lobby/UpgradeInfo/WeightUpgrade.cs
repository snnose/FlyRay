using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightUpgradeInfo : UpgradeInfo
{
    public WeightUpgradeInfo()
    {
        SetName("���� ����");
        SetDetail("���ּ��� ���԰� 25% �����մϴ�.");
        SetPrice(3500f);
    }
}

public class WeightUpgrade : MonoBehaviour
{
    UpgradeInfo upgradeInfo = new WeightUpgradeInfo();

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.playerData.weightUpgrade == 0)
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
