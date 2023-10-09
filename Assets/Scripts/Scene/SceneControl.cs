using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    public GameObject wafflePrefab;

    // Start is called before the first frame update
    void Start()
    {
        wafflePrefab = Resources.Load("Prefabs/Waffle") as GameObject;

        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnLocation =
                new Vector3(Random.Range(-3, 10), Random.Range(4, 12), 0);

            Instantiate(wafflePrefab, spawnLocation, wafflePrefab.transform.rotation);
        }
    }
}
