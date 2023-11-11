using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUIControl : MonoBehaviour
{
    public TextMeshProUGUI goldAmount;

    // Start is called before the first frame update
    void Start()
    {
        goldAmount.SetText(DataManager.Instance.playerData.currentGold.ToString());
        goldAmount.alignment = TMPro.TextAlignmentOptions.Center;
    }

    // Update is called once per frame
    void Update()
    {
        if (DataManager.Instance.IsGoldChanged())
            goldAmount.SetText(DataManager.Instance.playerData.currentGold.ToString());
    }
}
