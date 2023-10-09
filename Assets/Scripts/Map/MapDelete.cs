using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDelete : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isDelete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public bool isDelete(GameObject map)
    {
        bool ret = false;

        if (this.transform.position.x + 23.7 < player.transform.position.x - 23.7)
            ret = true;

        return ret;
    }
}
