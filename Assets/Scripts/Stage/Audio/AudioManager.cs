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
        
    }     
}
