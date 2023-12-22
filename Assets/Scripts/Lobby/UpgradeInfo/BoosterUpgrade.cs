using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUpgradeInfo : UpgradeInfo
{
    public BoosterUpgradeInfo()
    {
        SetName("�ν��� ����");
        SetDetail("CtrlŰ�� ���� �ν��͸� 1ȸ ����� �� �ֽ��ϴ�.");
        SetPrice(1000f);
    }
}

public class BoosterUpgrade : MonoBehaviour
{
    UpgradeInfo upgradeInfo = new BoosterUpgradeInfo();

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.playerData.boosterUpgrade == 0)
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
