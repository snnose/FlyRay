using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{
    private GameObject resultUI;
    private GameObject fuelUI;
    private GameObject DVAUI;
    public GameObject pauseUI;
    private GameObject player;

    private ResultUIControl resultUIControl;
    private FuelUIControl fuelUIControl;
    private DVAUIControl DVAUIControl;
    private PauseUIControl pauseUIControl;
    private PlayerControl playerControl;

    private float gold;
    private bool isActivateResult = false;

    private IEnumerator activateResultUI;

    // Start is called before the first frame update
    void Start()
    {
        gold = 0f;

        AttachComponents();
        resultUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.IsFly() || playerControl.IsLand())
            RenewUI();
        else
        {
            DVAUIControl.velocityText.text = "속도 :" + "\n" + "0 m/s";
            DVAUIControl.altitudeText.text = "높이 :" + "\n" + "0 m";
        }

        if (PauseControl.Instance.IsPause())
            pauseUI.SetActive(true);
        else
            pauseUI.SetActive(false);

        if (GameRoot.Instance.IsGameEnded() && !isActivateResult)
        {
            Invoke("InActivateUI", 0f);
            //Invoke("ActivateResultUI", 0f);

            activateResultUI = ActivateResultUI();
            StartCoroutine(activateResultUI);

            isActivateResult = true;
        }
    }

    private void AttachComponents()
    {
        this.resultUI = GameObject.FindGameObjectWithTag("ResultUI");
        this.fuelUI = GameObject.FindGameObjectWithTag("FuelUI");
        this.DVAUI = GameObject.FindGameObjectWithTag("DVAUI");
        this.player = GameObject.FindGameObjectWithTag("Player");

        this.resultUIControl = resultUI.GetComponent<ResultUIControl>();
        this.fuelUIControl = fuelUI.GetComponent<FuelUIControl>();
        this.DVAUIControl = DVAUI.GetComponent<DVAUIControl>();
        this.playerControl = player.GetComponent<PlayerControl>();
        this.pauseUIControl = pauseUI.GetComponent<PauseUIControl>();
    }

    private IEnumerator ActivateResultUI()
    {
        int waffleAmount = playerControl.GetPlayerInfo().GetWaffleCollected();
        float dis = Mathf.Ceil(player.transform.position.x * 100) / 20;
        float altitude = Mathf.Ceil(GameRoot.Instance.GetMaxAltitude() * 100) / 20 - 14f;

        gold = Mathf.Ceil((waffleAmount * 5 + Mathf.Ceil(dis * 0.06f)) * (1 + Mathf.Ceil(altitude) / 50000));

        resultUIControl.waffleCollected.text = 
            "얻은 와플 : " + waffleAmount + "개";
        resultUIControl.distance.text =
            "최대 거리 : " + dis + "m";
        resultUIControl.altitude.text =
            "최대 높이 : " + altitude + "m";
        resultUIControl.calculate.text = "\n(" + waffleAmount * 5 + " + " + dis + " x 0.06) x "
                                            + (1 + Mathf.Ceil((Mathf.Ceil(altitude) / 50000) * 100) / 100);

        if (altitude >= 5000f)
        {
            resultUIControl.calculate.text += " + 100";
            gold += 100;
        }

        resultUIControl.calculate.text += "\n\n";

        resultUIControl.totalScore.text = "총 " + gold;

        AudioManager.Instance.gameFinishSound.Play();
        resultUI.gameObject.SetActive(true);

        yield return null;
    }

    private void InActivateUI()
    {
        DVAUI.gameObject.SetActive(false);
        fuelUI.gameObject.SetActive(false);
    }

    private void RenewUI()
    {
        Rigidbody2D playerRb2D = player.GetComponent<Rigidbody2D>();

        float currPosX = player.transform.position.x;
        float currPosY = player.transform.position.y - 2.76f;
        float velocity = playerRb2D.velocity.x;

        DVAUIControl.distanceText.text = "거리 :" + "\n" + Mathf.Ceil(currPosX * 100) / 20 + " m";
        DVAUIControl.velocityText.text = "속도 :" + "\n" + Mathf.Ceil(velocity * 100) / 20 + " m/s";
        DVAUIControl.altitudeText.text = "높이 :" + "\n" + Mathf.Ceil(currPosY * 100) / 20 + " m";
    }

    public float GetWaffle()
    {
        if (this.gold < 0)
            this.gold = 0;

        return this.gold;
    }
}
