using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUIControl : MonoBehaviour
{
    public TextMeshProUGUI goldAmount;

    private float currentGold = 0f; // ÇöÀç °ñµå
    private float totalGold = 0f;   // ÃÑ °ñµå

    // Start is called before the first frame update
    void Start()
    {
        goldAmount.SetText(currentGold + " G");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
