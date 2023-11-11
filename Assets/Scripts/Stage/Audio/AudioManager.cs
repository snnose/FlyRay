using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource windSound;

    // Start is called before the first frame update
    void Start()
    {
        windSound = this.GetComponent<AudioSource>();
        windSound.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.Instance.IsFly())
        {
            windSound.volume = 0.1f * DataManager.Instance.playerData.effectValue;
        }

        if (PlayerControl.Instance.IsMaroPush())
        {
            windSound.volume = 0.2f * DataManager.Instance.playerData.effectValue;
        }
    }     
}
