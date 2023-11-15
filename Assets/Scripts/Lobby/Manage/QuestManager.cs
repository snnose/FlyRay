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

            string title = "목표 달성!";
            string content = "비행 중에 마로가 등장합니다.";
            noticeUIControl.SetNoticeText(title, content);
            DataManager.Instance.SaveData();
        }
    }

    public void GoalEnter()
    {
        Quest quest = new Quest();

        if (!DataManager.Instance.playerData.main1)
        {
            quest.SetTitle("첫 비행");
            quest.SetContent("고양이별로의 첫 걸음을 때세요." + "\n\n" + "(비행 횟수 0/1)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }
        else if (DataManager.Instance.playerData.main1)
        {
            quest.SetTitle("");
            quest.SetContent("");
        }

        //Debug.Log("퀘스트 UI 출력");
    }

    public void GoalExit()
    {
        if (noticeUI.activeSelf)
            noticeUI.SetActive(false);
    }
}
