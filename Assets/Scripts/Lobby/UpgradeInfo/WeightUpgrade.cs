using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightUpgradeInfo : UpgradeInfo
{
    public WeightUpgradeInfo()
    {
        SetName("무게 감소");
        SetDetail("우주선의 무게가 25% 감소합니다.");
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
