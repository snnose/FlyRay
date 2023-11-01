using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private GameObject infoDeliver;

    // Start is called before the first frame update
    void Start()
    {
        infoDeliver = GameObject.FindGameObjectWithTag("InfoDeliver");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Stage");
        DontDestroyOnLoad(infoDeliver);
    }
}
