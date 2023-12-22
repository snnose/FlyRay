using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    // �÷��� Ƚ��
    public int count;

    // ��Ÿ� �� ���� ����
    public float totalDistance; // �� ���� �Ÿ�
    public float maxDistance;   // �ְ� ��Ÿ�
    public float maxAltitude;   // �ְ� ����

    // ���� ��ǥ ���� ��Ȳ
    public bool main1;          // ���� 1
    public bool main2;
    public bool main3;
    public bool main4;
    public bool main5;

    // ���� ��Ȳ
    public float currentWaffle;   // ���� ���� ����
    public float totalWaffle;     // �� ���� ����

    // ���׷��̵� ��Ȳ (0�� ��Ȱ��, 1�� Ȱ��)
    public int throwUpgrade;    // ������ ���׷��̵�
    public int airdragUpgrade;  // ���� ���� ���� ����
    public int weightUpgrade;   // ���� ���� ���׷��̵�
    public int fuelUpgrade;     // ���� ���׷��̵�

    public int MaroUpgrade;     // ���� ���׷��̵�
    public int trumpetUpgrade;  // ���� ���׷��̵�

    public int chichuteUpgrade; // �� ���ϻ�
    public int boosterUpgrade;  // �ν���

    // ȯ�� ���� ��
    public float BGMValue;
    public float effectValue;
}

public class DataManager : MonoBehaviour
{
    // �̱���
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
            // �����
            
        }
        // ���̺� ������ ������
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

    // ������ �ʱ�ȭ
    public void InitData()
    {
        // ���� �ϼ�
        playerData.count = 0;
        // ��Ÿ� �� ���� ����
        playerData.totalDistance = 0f;
        playerData.maxDistance = 0f;
        playerData.maxAltitude = 0f;
        // ���� ��ǥ ���� ��Ȳ
        playerData.main1 = false;
        playerData.main2 = false;
        playerData.main3 = false;
        playerData.main4 = false;
        playerData.main5 = false;
        // ��ȭ
        playerData.currentWaffle = 0f;
        playerData.totalWaffle = 0f;
        // ���׷��̵�
        InitUpgrade();
        // ȯ�漳��
        playerData.BGMValue = 1f;
        playerData.effectValue = 1f;
    }

    // ���׷��̵� �ʱ�ȭ
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
