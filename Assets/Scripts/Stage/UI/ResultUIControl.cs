using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUIControl : MonoBehaviour
{
    public TextMeshProUGUI waffleCollected;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI totalScore;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
