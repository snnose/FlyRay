using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayUIControl : MonoBehaviour
{
    public TextMeshProUGUI day;

    // Start is called before the first frame update
    void Start()
    {
        int currentDay = DataManager.Instance.playerData.count;
        day.text = currentDay.ToString() + "ÀÏÂ÷";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
