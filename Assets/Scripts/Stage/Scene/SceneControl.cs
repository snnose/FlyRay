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
        //SetResolution();
    }

    // Update is called once per frame
    void Update()
    {
        //SetResolution();
    }

    void SetResolution()
    {
        int setWidth = 1366;
        int setHeight = 768;

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
    private void CalWaffle()
    {
        DataManager.Instance.GainGold(this.UIControl.GetWaffle());
    }

    private void CalRecord()
    {
        DataManager.Instance.playerData.count++;
        DataManager.Instance.playerData.totalDistance +=
            PlayerControl.Instance.GetPlayer().transform.position.x;

        if (PlayerControl.Instance.GetPlayer().transform.position.x * 5f > DataManager.Instance.playerData.maxDistance)
            DataManager.Instance.playerData.maxDistance = 
                Mathf.Round((PlayerControl.Instance.GetPlayer().transform.position.x * 100) / 20);
        if (GameRoot.Instance.GetMaxAltitude() * 5f> DataManager.Instance.playerData.maxAltitude)
            DataManager.Instance.playerData.maxAltitude = 
                Mathf.Round((GameRoot.Instance.GetMaxAltitude() * 100) / 20);
    }

    public void OnClickExit()
    {
        CalWaffle();
        CalRecord();
        DataManager.Instance.SaveData();
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickRestart()
    {
        CalWaffle();
        CalRecord();
        DataManager.Instance.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
