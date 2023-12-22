using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    private string qTitle;
    private string qContent;
    private string qDetail;

    public void SetTitle(string title)
    {
        this.qTitle = title;
    }

    public void SetContent(string content)
    {
        this.qContent = content;
    }

    public void SetDetail(string detail)
    {
        this.qDetail = detail;
    }

    public string GetTitle()
    {
        return this.qTitle;
    }

    public string GetContent()
    {
        return this.qContent;
    }
}

public class QuestManager : MonoBehaviour
{
    public GameObject noticeUI;
    public GameObject questUI;
    private NoticeUIControl noticeUIControl;
    private QuestUIControl questUIControl;

    private void Awake()
    {
        noticeUIControl = noticeUI.GetComponent<NoticeUIControl>();
        questUIControl = questUI.GetComponent<QuestUIControl>();
    }

    private void Start()
    {
        QuestClearCheck();
    }

    void Update()
    {
        QuestClearCheck();
    }

    private void QuestClearCheck()
    {
        // ���� �� �� ���� �� Ŭ����
        if (!DataManager.Instance.playerData.main1 &&
            DataManager.Instance.playerData.count == 1)
        {
            DataManager.Instance.playerData.main1 = true;
            noticeUI.SetActive(true);

            string title = "��ǥ �޼�!";
            string content = "���� - ���� 500��, ���� �߿� ���ΰ� �����մϴ�";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 500;
            DataManager.Instance.playerData.totalWaffle += 500;

            DataManager.Instance.SaveData();
        }
        // ������ ��ȭ �� Ŭ����
        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2 &&
            DataManager.Instance.playerData.throwUpgrade == 1)
        {
            DataManager.Instance.playerData.main2 = true;
            noticeUI.SetActive(true);

            string title = "��ǥ �޼�!";
            string content = "���� - ���� 300��, ���� �߿� ������ �����˴ϴ�";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 300;
            DataManager.Instance.playerData.totalWaffle += 300;

            DataManager.Instance.SaveData();
        }
        // 2500m �̻� ���� �� Ŭ����
        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3 &&
            DataManager.Instance.playerData.maxDistance >= 2500f)
        {
            DataManager.Instance.playerData.main3 = true;
            noticeUI.SetActive(true);

            string title = "��ǥ �޼�!";
            string content = "���� - ���� 500��";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 500;
            DataManager.Instance.playerData.totalWaffle += 500;

            DataManager.Instance.SaveData();
        }
        // �� 5000m ���� �� Ŭ����
        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4 &&
            DataManager.Instance.playerData.maxAltitude >= 5000f)
        {
            DataManager.Instance.playerData.main4 = true;
            noticeUI.SetActive(true);

            string title = "��ǥ �޼�!";
            string content = "�÷������ּż� �����մϴ�.";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.SaveData();
        }

        /*
        if (DataManager.Instance.playerData.main4 &&
           !DataManager.Instance.playerData.main5)
        {

        }
        */
    }

    public void GoalEnter()
    {
        Quest quest = new Quest();

        if (!DataManager.Instance.playerData.main1)
        {
            quest.SetTitle("ù ����!");
            quest.SetContent("ù ������ ������ ��ġ����." + "\n\n" + "(���� Ƚ�� 0/1)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2)
        {
            quest.SetTitle("���׷��̵�!");
            quest.SetContent("������ ��ȭ�� �����ϼ���." );

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3)
        {
            quest.SetTitle("�ָ��ָ�!");
            quest.SetContent("�� ���� 2,500���� �̻� �����ϼ���. \n\n (�ְ� �Ÿ� " +
                DataManager.Instance.playerData.maxDistance + "/2500)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4)
        {
            quest.SetTitle("����̺���!");
            quest.SetContent("�� 5000���͸� �����ϼ���. \n\n (�ְ� �� " +
                DataManager.Instance.playerData.maxAltitude + "/5000)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        /*
        if (DataManager.Instance.playerData.main4 &&
           !DataManager.Instance.playerData.main5)
        {
            quest.SetTitle("");
            quest.SetContent("");
        }
        */
    }

    public void GoalExit()
    {
        if (noticeUI.activeSelf)
            noticeUI.SetActive(false);
    }
}
