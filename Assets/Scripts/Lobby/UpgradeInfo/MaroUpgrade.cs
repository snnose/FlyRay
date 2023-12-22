using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaroUpgradeInfo : UpgradeInfo
{
    public MaroUpgradeInfo()
    {
        SetName("���� ��ȭ");
        SetDetail("������ ������ ���� 50% �����մϴ�.");
        SetPrice(500f);
    }
}

public class MaroUpgrade : MonoBehaviour
{
    UpgradeInfo upgradeInfo = new MaroUpgradeInfo();

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.playerData.MaroUpgrade == 0)
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
