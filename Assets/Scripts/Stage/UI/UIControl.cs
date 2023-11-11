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
    private GameObject player;

    private ResultUIControl resultUIControl;
    private FuelUIControl fuelUIControl;
    private DVAUIControl DVAUIControl;
    private PlayerControl playerControl;

    private float gold;

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
            DVAUIControl.velocityText.text = "Velocity :" + "\n" + "0 m/s";
            DVAUIControl.altitudeText.text = "Altitude :" + "\n" + "0 m";
        }

        if (playerControl.IsStop())
        {
            Invoke("InActivateUI", 0f);
            Invoke("ActivateResultUI", 1f);
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
    }

    private void ActivateResultUI()
    {
        int waffleAmount = playerControl.GetPlayerInfo().GetWaffleCollected();
        float dis = Mathf.Ceil(player.transform.position.x * 100) / 100;

        gold = (waffleAmount * 5 + Mathf.Ceil(dis * 0.3f));

        resultUIControl.waffleCollected.text = 
            "Gain waffles = " + waffleAmount + "              " + waffleAmount * 5 + " $";
        resultUIControl.distance.text = 
            "Distance = " + dis + " m" + "        " + Mathf.Ceil(dis * 0.3f * 10) / 10 + " $";
        resultUIControl.totalScore.text = "Total = " + gold + " $";

        resultUI.gameObject.SetActive(true);
    }

    private void InActivateUI()
    {
        DVAUI.gameObject.SetActive(false);
        fuelUI.gameObject.SetActive(false);
    }

    void RenewUI()
    {
        Rigidbody2D playerRb2D = player.GetComponent<Rigidbody2D>();

        float currPosX = player.transform.position.x + 5f;
        float currPosY = player.transform.position.y - 2.76f;
        float velocity = playerRb2D.velocity.x;

        DVAUIControl.distanceText.text = "Distance :" + "\n" + Mathf.Ceil(currPosX * 100) / 100 + " m";
        DVAUIControl.velocityText.text = "Velocity :" + "\n" + Mathf.Ceil(velocity * 100) / 100 + " m/s";
        DVAUIControl.altitudeText.text = "Altitude :" + "\n" + Mathf.Ceil(currPosY * 100) / 100 + " m";
    }

    public float GetGold()
    {
        if (this.gold < 0)
            this.gold = 0;

        return this.gold;
    }
}
