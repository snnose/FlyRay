using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUIControl : MonoBehaviour
{
    public TextMeshProUGUI waffleCollected;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI distanceNewRecord;
    public TextMeshProUGUI altitude;
    public TextMeshProUGUI altitudeNewRecord;
    public TextMeshProUGUI calculate;
    public TextMeshProUGUI totalScore;

    private float maxAltitude = 0f;

    // Start is called before the first frame update
    void Start()
    {
        distanceNewRecord.alpha = 0f;
        altitudeNewRecord.alpha = 0f;
        this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
