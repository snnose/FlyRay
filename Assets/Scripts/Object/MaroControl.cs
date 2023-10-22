using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaroControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0.01f, (float)Mathf.Sin(Time.time) * Time.deltaTime, 0f));
    }
}
