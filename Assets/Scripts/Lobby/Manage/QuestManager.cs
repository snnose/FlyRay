using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    private string qTitle;
    private string qContent;

    public void SetTitle(string title)
    {
        this.qTitle = title;
    }

    public void SetContent(string content)
    {
        this.qContent = content;
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
    private NoticeUIControl noticeUIControl;

    private void Awake()
    {
        noticeUIControl = noticeUI.GetComponent<NoticeUIControl>();
    }

    void Start()
    {
        QuestCheck();
    }

    private void QuestCheck()
    {
        if (!DataManager.Instance.playerData.main1 &&
            DataManager.Instance.playerData.count == 1)
        {
            DataManager.Instance.playerData.main1 = true;
            noticeUI.SetActive(true);

            string title = "��ǥ �޼�!";
            string content = "���� �߿� ���ΰ� �����մϴ�.";
            noticeUIControl.SetNoticeText(title, content);
            DataManager.Instance.SaveData();
        }
    }

    public void GoalEnter()
    {
        Quest quest = new Quest();

        if (!DataManager.Instance.playerData.main1)
        {
            quest.SetTitle("ù ����");
            quest.SetContent("����̺����� ù ������ ������." + "\n\n" + "(���� Ƚ�� 0/1)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }
        else if (DataManager.Instance.playerData.main1)
        {
            quest.SetTitle("");
            quest.SetContent("");
        }

        //Debug.Log("����Ʈ UI ���");
    }

    public void GoalExit()
    {
        if (noticeUI.activeSelf)
            noticeUI.SetActive(false);
    }
}
