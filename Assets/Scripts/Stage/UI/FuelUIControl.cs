using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelUIControl : MonoBehaviour
{
    public TextMeshProUGUI fuelName;
    public Image fuelGage;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Screen.width * 0.125f, Screen.height * 0.05f);
        fuelName.transform.position += new Vector3(0f, 40f);

        fuelName.text = "¿¬·á";
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
