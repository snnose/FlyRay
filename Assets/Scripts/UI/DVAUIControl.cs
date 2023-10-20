using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DVAUIControl : MonoBehaviour
{
    public Image DVABackground;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI altitudeText;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Screen.width * 0.30f, Screen.height * 0.90f);

        distanceText.text = "Distance : 0 m";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
