using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelUIControl : MonoBehaviour
{
    public TextMeshProUGUI fuelName;
    public Image fuelGage;

    private GameObject canvas;
    private GameObject fuelUI;

    private float canvasX;
    private float canvasY;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        fuelUI = GameObject.FindGameObjectWithTag("FuelUI");

        canvasX = canvas.transform.position.x;
        canvasY = canvas.transform.position.y;

        this.transform.position += new Vector3(-canvasX * 0.65f,
                                               -canvasY * 0.9f);
        fuelName.transform.position += new Vector3(0f, 20f);

        fuelName.text = "Fuel";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Image GetFuelGage()
    {
        return this.fuelGage;
    }
}
