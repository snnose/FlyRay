using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }
    public AudioSource windSound;
    public AudioSource grabSound;
    public AudioSource waffleSound;
    public AudioSource trumpetSound;
    public AudioSource chicuteSound;
    public AudioSource spaceEngineSound;
    public AudioSource gameFinishSound;
    public AudioSource maroSound;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        InitSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.Instance.IsFly())
        {
            windSound.volume = DataManager.Instance.playerData.effectValue * 0.3f;
        }

        if (PlayerControl.Instance.IsMaroPush())
        {
            windSound.volume = DataManager.Instance.playerData.effectValue * 0.45f;
        }

        if (GameRoot.Instance.IsGameEnded())
        {
            windSound.Stop();
        }
    }

    // �Ҹ� �ʱ�ȭ
    private void InitSound()
    {
        windSound.volume = 0f;
        grabSound.volume = DataManager.Instance.playerData.effectValue;
        waffleSound.volume = DataManager.Instance.playerData.effectValue;
        trumpetSound.volume = DataManager.Instance.playerData.effectValue;
        chicuteSound.volume = DataManager.Instance.playerData.effectValue * 1.5f;
        spaceEngineSound.volume = DataManager.Instance.playerData.effectValue * 2;
        gameFinishSound.volume = DataManager.Instance.playerData.effectValue;
        maroSound.volume = DataManager.Instance.playerData.effectValue * 1.5f;
    }
}
