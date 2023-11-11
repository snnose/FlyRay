using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    private static PauseControl instance;

    public static PauseControl Instance
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
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPause())
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPause = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPause = false;
    }

    public bool IsPause()
    {
        return this.isPause;
    }
}
