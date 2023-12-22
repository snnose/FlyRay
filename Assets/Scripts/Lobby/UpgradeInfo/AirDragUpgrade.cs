using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirDragUpgradeInfo : UpgradeInfo
{
    public AirDragUpgradeInfo()
    {
        SetName("������ ��");
        SetDetail("�޴� ���� ������ 30% �����մϴ�.");
        SetPrice(500f);
    }
}

public class AirDragUpgrade : MonoBehaviour
{
    private UpgradeInfo upgradeInfo = new AirDragUpgradeInfo();

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.playerData.airdragUpgrade == 0)
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
