using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeUIControl : MonoBehaviour
{
    public GameObject noticeUI;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    private void Start()
    {
        noticeUI.transform.position = new Vector3(960, 540, 0);
        noticeUI.SetActive(false);
    }

    public void SetNoticeText(string t, string c)
    {
        title.text = t;
        content.text = c;
    }

    public void OnClickExit()
    {
        noticeUI.SetActive(false);
    }
}
