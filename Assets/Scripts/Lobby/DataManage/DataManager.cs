using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    // 진행 일 수 (플레이 횟수)
    int day;

    // 골드 현황
    public float currentGold;
    public float totalGold;

    // 업그레이드 현황 (0은 비활성, 1은 활성)
    public int throwUpgrade;    // 던지기 업그레이드
    public int fuelUpgrade;     // 연료 업그레이드

    public int chichute;         // 닭 낙하산

    // 환경 설정 값
    public float BGMValue;
    public float effectValue;
}

public class DataManager : MonoBehaviour
{
    // 싱글톤
    private static DataManager instance = null;

    public static DataManager Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        path = Application.persistentDataPath + "/save";

        if (File.Exists(path))
        {
            LoadData();
            // 디버그
            
        }
        // 세이브 파이리 없으면
        else
        {
            // 데이터 초기화
            // 재화
            playerData.currentGold = 0f;
            playerData.totalGold = 0f;
            // 업그레이드
            playerData.throwUpgrade = 0;
            playerData.fuelUpgrade = 0;
            // 환경설정
            playerData.BGMValue = 1f;
            playerData.effectValue = 1f;
        }
    }

    public PlayerData playerData = new PlayerData();

    bool goldChanged = false;
    string path;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }

    public void GainGold(float gold)
    {
        playerData.currentGold = Mathf.Ceil(playerData.currentGold);
        playerData.totalGold = Mathf.Ceil(playerData.totalGold);

        playerData.currentGold += gold;
        playerData.totalGold += gold;
    }

    public void SpendGold(float gold)
    {
        playerData.currentGold -= gold;
        this.goldChanged = true;
    }

    public bool IsGoldChanged()
    {
        bool ret = false;

        if (this.goldChanged)
        {
            ret = true;
            goldChanged = false;
        }

        return ret;
    }

    // 업그레이드 초기화
    public void InitUpgrade()
    {
        playerData.throwUpgrade = 0;
        playerData.fuelUpgrade = 0;

        playerData.currentGold = playerData.totalGold;
        this.goldChanged = true;

        SaveData();
    }
}
