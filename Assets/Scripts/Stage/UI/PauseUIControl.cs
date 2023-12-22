using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseUIControl : MonoBehaviour
{
    public GameObject pauseUI;
    public TextMeshProUGUI retry;
    public TextMeshProUGUI exit;

    private void Awake()
    { 
        pauseUI.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, -1);
        pauseUI.SetActive(false);
    }

    public void OnClickRetry()
    {
        PauseControl.Instance.SetPause(false);
        pauseUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickExit()
    {
        GameRoot.Instance.SetGameEnded(true);
        PauseControl.Instance.SetPause(false);
        pauseUI.SetActive(false);
    }
}
