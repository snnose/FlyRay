using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SettingUIControl : MonoBehaviour
{
    private GameObject settingUI;
    public GameObject BGMScrollBar;
    public GameObject effectScrollBar;

    public TextMeshProUGUI BGMText;
    public TextMeshProUGUI effectText;

    private void Awake()
    {
        this.settingUI = GameObject.FindGameObjectWithTag("SettingUI");
        this.settingUI.transform.position = new Vector3(960, 540, 0);
        
        this.settingUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSetting()
    {
        float bgmValue = DataManager.Instance.playerData.BGMValue;
        float effectValue = DataManager.Instance.playerData.effectValue;

        BGMScrollBar.GetComponent<Scrollbar>().value = bgmValue;
        effectScrollBar.GetComponent<Scrollbar>().value = effectValue;

        BGMText.text = "배경음 - " + (bgmValue * 100).ToString() + "%";
        effectText.text = "효과음 - " +  (effectValue * 100).ToString() + "%";

        settingUI.SetActive(true);
    }

    public void OnClickExit()
    {
        settingUI.SetActive(false);
        DataManager.Instance.SaveData();
    }

    public void OnClickDataInit()
    {
        DataManager.Instance.InitData();
        DataManager.Instance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    public void OnValueChangedBGM()
    {
        
        GameObject scrollbar =
            EventSystem.current.currentSelectedGameObject;

        try
        {
            float value = Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 100) / 100;

            DataManager.Instance.playerData.BGMValue = value;
            BGMText.text = "배경음 - " + (value * 100).ToString() + "%";
        }
        catch (NullReferenceException exception)
        {
            
        }
    }

    public void OnValueChangedEffect()
    {
        GameObject scrollbar =
            EventSystem.current.currentSelectedGameObject;
        try
        {
            float value = Mathf.Round(scrollbar.GetComponent<Scrollbar>().value * 100) / 100;

            DataManager.Instance.playerData.effectValue = value;
            effectText.text = "효과음 - " + (value * 100).ToString() + "%";
        }
        catch (NullReferenceException exception)
        {
            
        }
    }
}
