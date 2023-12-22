using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
                return null;

            return instance;
        }
    }

    public AudioSource clickSound;
    public AudioSource exitClickSound;
    public AudioSource IconClickSound;
    public AudioSource purchaseClickSound;

    private void Update()
    {
        clickSound.volume = DataManager.Instance.playerData.effectValue;
        exitClickSound.volume = DataManager.Instance.playerData.effectValue;
        IconClickSound.volume = DataManager.Instance.playerData.effectValue * 0.5f;
        purchaseClickSound.volume = DataManager.Instance.playerData.effectValue;
    }

    public void OnPointerClick()
    {
        clickSound.Play();
    }

    public void OnPointerExitClick()
    {
        exitClickSound.Play();
    }

    public void OnPointerIconClick()
    {
        IconClickSound.Play();
    }

    public void OnPointerPurchaseClick()
    {
        purchaseClickSound.Play();
    }
}
