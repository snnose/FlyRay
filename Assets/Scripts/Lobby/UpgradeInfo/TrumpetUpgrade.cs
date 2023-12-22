using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrumpetUpgradeInfo : UpgradeInfo
{
    public TrumpetUpgradeInfo()
    {
        SetName("나팔 강화");
        SetDetail("나팔 효과가 50% 증가합니다.");
        SetPrice(500f);
    }
}

public class TrumpetUpgrade : MonoBehaviour
{
    private UpgradeInfo upgradeInfo = new TrumpetUpgradeInfo();

    private void Start()
    {
        if (DataManager.Instance.playerData.trumpetUpgrade == 0)
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
