using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIControl : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    void Update()
    {
        if (!DataManager.Instance.playerData.main1)
        {
            title.text = "첫 비행!";
            content.text = "밍";
        }

        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2)
        {
            title.text = "업그레이드!";
            content.text = "밍밍";
        }

        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3)
        {
            title.text = "멀리멀리!";
            content.text = "밍밍밍";
        }

        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4)
        {
            title.text = "고양이별로!";
            content.text = "흠 흐밍";
        }

        if (DataManager.Instance.playerData.main4)
        {
            title.text = "";
            content.text = "-왕-";
        }
    }

    public void SetQuestText(string title, string content)
    {
        this.title.text = title;
        this.content.text = content;
    }
}
