using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDelete : MonoBehaviour
{
    private GameObject player;

    private BoxCollider2D mapCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mapCollider2D = this.GetComponent<BoxCollider2D>();
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

        if (this.transform.position.x + (mapCollider2D.size.x / 2) <
            player.transform.position.x - (mapCollider2D.size.x / 2))            
            ret = true;

        return ret;
    }
}
