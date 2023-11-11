using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    // ���� �� �� (�÷��� Ƚ��)
    int day;

    // ��� ��Ȳ
    public float currentGold;
    public float totalGold;

    // ���׷��̵� ��Ȳ (0�� ��Ȱ��, 1�� Ȱ��)
    public int throwUpgrade;    // ������ ���׷��̵�
    public int fuelUpgrade;     // ���� ���׷��̵�

    public int chichute;         // �� ���ϻ�

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
        // ���̺� ���̸� ������
        else
        {
            // ������ �ʱ�ȭ
            // ��ȭ
            playerData.currentGold = 0f;
            playerData.totalGold = 0f;
            // ���׷��̵�
            playerData.throwUpgrade = 0;
            playerData.fuelUpgrade = 0;
            // ȯ�漳��
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

    // ���׷��̵� �ʱ�ȭ
    public void InitUpgrade()
    {
        playerData.throwUpgrade = 0;
        playerData.fuelUpgrade = 0;

        playerData.currentGold = playerData.totalGold;
        this.goldChanged = true;

        SaveData();
    }
}
