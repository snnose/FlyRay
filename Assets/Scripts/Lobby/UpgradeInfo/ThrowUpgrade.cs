using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowUpgradeInfo : UpgradeInfo
{
    public ThrowUpgradeInfo()
    {
        SetName("던지기 강화");
        SetDetail("던지는 힘이 50% 증가합니다.");
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
