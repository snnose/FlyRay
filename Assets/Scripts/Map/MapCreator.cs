using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject player;
    public GameObject backGroundPrefab;
    public GameObject groundPrefab;

    float initRightPosX = 0f;
    float xScreenHalfSize;
    float yScreenHalfSize;

    Vector3 currGroundPos;
    Vector3 currBackGroundPos;

    float currRightPosX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        // 게임 시작 화면의 좌측 우측 좌표
        currRightPosX = initRightPosX = xScreenHalfSize * 2 - 1;

        currGroundPos = groundPrefab.transform.position;
        currBackGroundPos = backGroundPrefab.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > currRightPosX)
        {
            //Debug.Log("rightPosX = " + currRightPosX);
            currRightPosX += initRightPosX * 2;

            GameObject go1 =
                Instantiate(groundPrefab) as GameObject;

            GameObject go2 =
                Instantiate(backGroundPrefab) as GameObject;

            currGroundPos.x += groundPrefab.GetComponent<BoxCollider2D>().size.x;
            currBackGroundPos.x += backGroundPrefab.GetComponent<BoxCollider2D>().size.x;

            go1.transform.position = currGroundPos;
            go2.transform.position = currBackGroundPos;
        }
    }
}
