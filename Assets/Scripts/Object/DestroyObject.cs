using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    float yScreenHalfSize;
    float xScreenHalfSize;

    Vector3 cameraPos;
    float xDestroyPos;

    // Start is called before the first frame update
    void Start()
    {
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPos = Camera.main.transform.position;

        xDestroyPos = cameraPos.x - (xScreenHalfSize * 2) - 1;

        if (this.transform.position.x < xDestroyPos || this.transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
