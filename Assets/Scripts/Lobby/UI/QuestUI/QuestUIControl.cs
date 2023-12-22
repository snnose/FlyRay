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
            title.text = "ù ����!";
            content.text = "��";
        }

        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2)
        {
            title.text = "���׷��̵�!";
            content.text = "�ֹ�";
        }

        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3)
        {
            title.text = "�ָ��ָ�!";
            content.text = "�ֹֹ�";
        }

        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4)
        {
            title.text = "����̺���!";
            content.text = "�� ���";
        }

        if (DataManager.Instance.playerData.main4)
        {
            title.text = "";
            content.text = "-��-";
        }
    }

    public void SetQuestText(string title, string content)
    {
        this.title.text = title;
        this.content.text = content;
    }
}
