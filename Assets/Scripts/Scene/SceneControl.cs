using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    public GameObject wafflePrefab;

    // Start is called before the first frame update
    void Start()
    {
        wafflePrefab = Resources.Load("Prefabs/Waffle") as GameObject;

        SetResolution();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnLocation =
                new Vector3(Random.Range(-3, 10), Random.Range(4, 12), 0);

            Instantiate(wafflePrefab, spawnLocation, wafflePrefab.transform.rotation);
        }
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
}
