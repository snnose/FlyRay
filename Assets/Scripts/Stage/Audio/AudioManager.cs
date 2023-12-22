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
            windSound.volume = DataManager.Instance.playerData.effectValue;
        }

        if (PlayerControl.Instance.IsMaroPush())
        {
            windSound.volume = DataManager.Instance.playerData.effectValue * 1.5f;
        }

        if (GameRoot.Instance.IsGameEnded())
        {
            windSound.Stop();
        }
    }

    // 소리 초기화
    private void InitSound()
    {
        windSound.volume = 0f;
        grabSound.volume = DataManager.Instance.playerData.effectValue;
        waffleSound.volume = DataManager.Instance.playerData.effectValue;
        trumpetSound.volume = DataManager.Instance.playerData.effectValue;
        chicuteSound.volume = DataManager.Instance.playerData.effectValue;
        spaceEngineSound.volume = DataManager.Instance.playerData.effectValue * 2;
        gameFinishSound.volume = DataManager.Instance.playerData.effectValue;
    }
}
