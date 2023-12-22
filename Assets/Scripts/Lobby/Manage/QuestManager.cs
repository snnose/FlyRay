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
        // 게임 한 판 진행 시 클리어
        if (!DataManager.Instance.playerData.main1 &&
            DataManager.Instance.playerData.count == 1)
        {
            DataManager.Instance.playerData.main1 = true;
            noticeUI.SetActive(true);

            string title = "목표 달성!";
            string content = "보상 - 와플 500개, 비행 중에 마로가 등장합니다";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 500;
            DataManager.Instance.playerData.totalWaffle += 500;

            DataManager.Instance.SaveData();
        }
        // 던지기 강화 시 클리어
        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2 &&
            DataManager.Instance.playerData.throwUpgrade == 1)
        {
            DataManager.Instance.playerData.main2 = true;
            noticeUI.SetActive(true);

            string title = "목표 달성!";
            string content = "보상 - 와플 300개, 비행 중에 나팔이 생성됩니다";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 300;
            DataManager.Instance.playerData.totalWaffle += 300;

            DataManager.Instance.SaveData();
        }
        // 2500m 이상 비행 시 클리어
        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3 &&
            DataManager.Instance.playerData.maxDistance >= 2500f)
        {
            DataManager.Instance.playerData.main3 = true;
            noticeUI.SetActive(true);

            string title = "목표 달성!";
            string content = "보상 - 와플 500개";
            noticeUIControl.SetNoticeText(title, content);

            DataManager.Instance.playerData.currentWaffle += 500;
            DataManager.Instance.playerData.totalWaffle += 500;

            DataManager.Instance.SaveData();
        }
        // 고도 5000m 도달 시 클리어
        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4 &&
            DataManager.Instance.playerData.maxAltitude >= 5000f)
        {
            DataManager.Instance.playerData.main4 = true;
            noticeUI.SetActive(true);

            string title = "목표 달성!";
            string content = "플레이해주셔서 감사합니다.";
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
            quest.SetTitle("첫 비행!");
            quest.SetContent("첫 비행을 무사히 마치세요." + "\n\n" + "(비행 횟수 0/1)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main1 &&
           !DataManager.Instance.playerData.main2)
        {
            quest.SetTitle("업그레이드!");
            quest.SetContent("던지기 강화를 구매하세요." );

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main2 &&
           !DataManager.Instance.playerData.main3)
        {
            quest.SetTitle("멀리멀리!");
            quest.SetContent("한 번에 2,500미터 이상 비행하세요. \n\n (최고 거리 " +
                DataManager.Instance.playerData.maxDistance + "/2500)");

            noticeUI.SetActive(true);
            noticeUIControl.SetNoticeText(quest.GetTitle(), quest.GetContent());
        }

        if (DataManager.Instance.playerData.main3 &&
           !DataManager.Instance.playerData.main4)
        {
            quest.SetTitle("고양이별로!");
            quest.SetContent("고도 5000미터를 도달하세요. \n\n (최고 고도 " +
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
