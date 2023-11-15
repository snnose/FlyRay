using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIControl : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    void Start()
    {
        if (!DataManager.Instance.playerData.main1)
        {
            title.text = "ù ����";
            content.text = "��";
        }
        else if (DataManager.Instance.playerData.main1)
        {
            title.text = "";
            content.text = "";
        }
    }
}
