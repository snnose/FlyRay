using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private GameObject canvas;
  
    private UIControl UIControl;

    // Start is called before the first frame update
    void Start()
    {
        SetResolution();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        // 기기의 해상도 비가 더 크다면
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        // 게임의 해상도 비가 더 크다면
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        UIControl = canvas.GetComponent<UIControl>();
    }

    // 골드 정산
    private void CalGold()
    {
        DataManager.Instance.GainGold(this.UIControl.GetGold());
    }

    public void OnClickExit()
    {
        CalGold();
        DataManager.Instance.SaveData();
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickRestart()
    {
        CalGold();
        DataManager.Instance.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
