using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private GameObject player;
    private GameObject copy;

    private float spawnDelay = 0f;
    private List<float> spawnInterval;

    // 코루틴 함수
    IEnumerator spawnWaffle;
    IEnumerator spawnMaro;
    IEnumerator spawnTrumpet;

    // 카메라 기준 중앙 좌표
    float yScreenHalfSize;
    float xScreenHalfSize;

    private bool isExcute = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spawnInterval = new List<float>(new float[] { 0.1f , 0.75f, 0.75f});

        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        SpawnInit();
        spawnWaffle = SpawnWaffle();
        spawnMaro = SpawnMaro();
        spawnTrumpet = SpawnTrumpet();
    }

    private void Update()
    {
        if (!isExcute && player.GetComponent<PlayerControl>().IsFly())
        {
            isExcute = true;
            
            StartCoroutine(spawnWaffle);

            if (DataManager.Instance.playerData.main1)
            {
                StartCoroutine(spawnMaro);
            }

            if (DataManager.Instance.playerData.main2)
            {
                StartCoroutine(spawnTrumpet);
            }
        }
    }

    void SpawnInit()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnLocation =
                new Vector3(Random.Range(1, 10), Random.Range(4, 12), 0);

            Instantiate(objectPrefabs[0], spawnLocation, objectPrefabs[0].transform.rotation);
        }
    }


    private IEnumerator SpawnWaffle()
    {
        while (!GameRoot.Instance.IsGameEnded())
        {
            float xSpawnPos = player.transform.position.x + Random.Range(5, 50);
            float ySpawnPos = player.transform.position.y +
                                Random.Range(-25, 25);

            Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

            if (player.GetComponent<PlayerControl>().IsFly() ||
                player.GetComponent<PlayerControl>().IsLand())
            {
                GameObject copy = Instantiate(objectPrefabs[0], spawnLocation,
                    objectPrefabs[0].transform.rotation);
            }

            yield return new WaitForSeconds(spawnInterval[0]);
        }
    }

    private IEnumerator SpawnMaro()
    {
        while (!GameRoot.Instance.IsGameEnded())
        {
            float xSpawnPos = player.transform.position.x +
                                xScreenHalfSize * 2 + Random.Range(1, 20);
            float ySpawnPos = player.transform.position.y +
                                Random.Range(-25, 25);

            Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

            if (player.GetComponent<PlayerControl>().IsFly() ||
                player.GetComponent<PlayerControl>().IsLand())
            {
                GameObject copy = Instantiate(objectPrefabs[1], spawnLocation,
                    objectPrefabs[1].transform.rotation);
            }

            yield return new WaitForSeconds(spawnInterval[1]);
        }
    }

    private IEnumerator SpawnTrumpet()
    {
        while (!GameRoot.Instance.IsGameEnded())
        {
            float xSpawnPos = player.transform.position.x +
                                xScreenHalfSize * 2 + Random.Range(1, 20);
            float ySpawnPos = player.transform.position.y +
                                Random.Range(-25, 25);

            Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

            if (player.GetComponent<PlayerControl>().IsFly() ||
                player.GetComponent<PlayerControl>().IsLand())
            {
                GameObject copy = Instantiate(objectPrefabs[2], spawnLocation,
                    objectPrefabs[2].transform.rotation);
            }

            yield return new WaitForSeconds(spawnInterval[2]);
        }
    }
    /*
    void SpawnChicken()
    {
        float xSpawnPos = player.transform.position.x +
                            xScreenHalfSize * 2 + Random.Range(1, 40);
        float ySpawnPos = 2.49f;

        Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

        if (player.GetComponent<PlayerControl>().IsFly() ||
            player.GetComponent<PlayerControl>().IsLand())
        {
            GameObject copy = Instantiate(objectPrefabs[2], spawnLocation,
                objectPrefabs[2].transform.rotation);
        }
    }
    */
}
