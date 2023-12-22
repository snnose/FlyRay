using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    // 플레이 횟수
    public int count;

    // 비거리 및 높이 정보
    public float totalDistance; // 총 진행 거리
    public float maxDistance;   // 최고 비거리
    public float maxAltitude;   // 최고 높이

    // 메인 목표 진행 현황
    public bool main1;          // 메인 1
    public bool main2;
    public bool main3;
    public bool main4;
    public bool main5;

    // 와플 현황
    public float currentWaffle;   // 현재 와플 개수
    public float totalWaffle;     // 총 와플 개수

    // 업그레이드 현황 (0은 비활성, 1은 활성)
    public int throwUpgrade;    // 던지기 업그레이드
    public int airdragUpgrade;  // 공기 저항 감소 업글
    public int weightUpgrade;   // 무게 감소 업그레이드
    public int fuelUpgrade;     // 연료 업그레이드

    public int MaroUpgrade;     // 마로 업그레이드
    public int trumpetUpgrade;  // 나팔 업그레이드

    public int chichuteUpgrade; // 닭 낙하산
    public int boosterUpgrade;  // 부스터

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
        // 세이브 파일이 없으면
        else
        {
            InitData();
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
        playerData.currentWaffle = Mathf.Ceil(playerData.currentWaffle);
        playerData.totalWaffle = Mathf.Ceil(playerData.totalWaffle);

        playerData.currentWaffle += gold;
        playerData.totalWaffle += gold;
    }

    public void SpendGold(float gold)
    {
        playerData.currentWaffle -= gold;
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

    // 데이터 초기화
    public void InitData()
    {
        // 진행 일수
        playerData.count = 0;
        // 비거리 및 높이 정보
        playerData.totalDistance = 0f;
        playerData.maxDistance = 0f;
        playerData.maxAltitude = 0f;
        // 메인 목표 진행 현황
        playerData.main1 = false;
        playerData.main2 = false;
        playerData.main3 = false;
        playerData.main4 = false;
        playerData.main5 = false;
        // 재화
        playerData.currentWaffle = 0f;
        playerData.totalWaffle = 0f;
        // 업그레이드
        InitUpgrade();
        // 환경설정
        playerData.BGMValue = 1f;
        playerData.effectValue = 1f;
    }

    // 업그레이드 초기화
    public void InitUpgrade()
    {
        playerData.throwUpgrade = 0;
        playerData.airdragUpgrade = 0;
        playerData.weightUpgrade = 0;
        playerData.fuelUpgrade = 0;

        playerData.MaroUpgrade = 0;
        playerData.trumpetUpgrade = 0;

        playerData.chichuteUpgrade = 0;
        playerData.boosterUpgrade = 0;
    }

    public void InitGold()
    {
        playerData.currentWaffle = playerData.totalWaffle;
        this.goldChanged = true;
    }
}
