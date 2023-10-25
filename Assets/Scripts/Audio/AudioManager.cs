using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private GameObject gameRoot;
    private AudioSource windSound;

    private GameRoot gameRootScript;

    // Start is called before the first frame update
    void Start()
    {
        windSound = this.GetComponent<AudioSource>();
        windSound.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }     
}
