using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject player;
    private GameObject mapPrefab;

    Vector2 currMapPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mapPrefab = Resources.Load("Prefabs/Map") as GameObject;
        currMapPos = mapPrefab.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > currMapPos.x)
        {
            currMapPos.x += mapPrefab.GetComponent<BoxCollider2D>().size.x;

            GameObject go1 =
                Instantiate(mapPrefab) as GameObject;

            go1.transform.position = currMapPos;
        }
    }
}
