using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChichuteUpgradeInfo : UpgradeInfo
{
    public ChichuteUpgradeInfo()
    {
        SetName("닭하산 장착");
        SetDetail("하강 중 X키를 누르면 천천히 하강합니다.");
        SetPrice(500f);
    }
}

public class ChichuteUpgrade : MonoBehaviour
{
    UpgradeInfo upgradeInfo = new ChichuteUpgradeInfo();

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.playerData.chichuteUpgrade == 0)
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
