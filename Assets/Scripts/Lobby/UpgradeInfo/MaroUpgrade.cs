using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaroUpgradeInfo : UpgradeInfo
{
    public MaroUpgradeInfo()
    {
        SetName("마로 강화");
        SetDetail("마로의 던지는 힘이 50% 증가합니다.");
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
