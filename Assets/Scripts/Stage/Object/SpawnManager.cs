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

    // Ä«¸Þ¶ó ±âÁØ Áß¾Ó ÁÂÇ¥
    float yScreenHalfSize;
    float xScreenHalfSize;

    private bool isExcute = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spawnInterval = new List<float>(new float[] { 0.1f , 0.5f, 4.0f});

        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        SpawnInit();
    }

    private void Update()
    {
        if (!isExcute && player.GetComponent<PlayerControl>().IsFly())
        {
            isExcute = true;
            InvokeRepeating("SpawnWaffle", spawnDelay, spawnInterval[0]);
            InvokeRepeating("SpawnMaro", spawnDelay, spawnInterval[1]);
            InvokeRepeating("SpawnChicken", spawnDelay, spawnInterval[2]);
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

    void SpawnWaffle()
    {
        float xSpawnPos = player.transform.position.x + 
                            xScreenHalfSize * 2 + Random.Range(1, 10);
        float ySpawnPos = player.transform.position.y + 
                            Random.Range(-player.transform.position.y + 2, 20);

        Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

        if (player.GetComponent<PlayerControl>().IsFly() ||
            player.GetComponent<PlayerControl>().IsLand())
        {
            GameObject copy = Instantiate(objectPrefabs[0], spawnLocation,
                objectPrefabs[0].transform.rotation);
        }
    }

    void SpawnMaro()
    {
        float xSpawnPos = player.transform.position.x +
                            xScreenHalfSize * 2 + Random.Range(1, 10);
        float ySpawnPos = player.transform.position.y +
                            Random.Range(-player.transform.position.y + 2, 20);

        Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

        if (player.GetComponent<PlayerControl>().IsFly() ||
            player.GetComponent<PlayerControl>().IsLand())
        {
            GameObject copy = Instantiate(objectPrefabs[1], spawnLocation,
                objectPrefabs[1].transform.rotation);
        }
    }

    void SpawnChicken()
    {
        float xSpawnPos = player.transform.position.x +
                            xScreenHalfSize * 2 + Random.Range(1, 10);
        float ySpawnPos = 2.49f;

        Vector3 spawnLocation = new Vector3(xSpawnPos, ySpawnPos, 0);

        if (player.GetComponent<PlayerControl>().IsFly() ||
            player.GetComponent<PlayerControl>().IsLand())
        {
            GameObject copy = Instantiate(objectPrefabs[2], spawnLocation,
                objectPrefabs[2].transform.rotation);
        }
    }
}
