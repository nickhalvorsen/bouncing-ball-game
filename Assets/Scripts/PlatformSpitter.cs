using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpitter : MonoBehaviour
{

    public GameObject prefab; 
    private Vector3 spawnPoint = new Vector3(0, 0, 15);
    private float spawnHorizontalVariance = 20f;
    private float spawnVerticalVariance = 0.1f;
    public float spawnRate = 2f; // Number of spawns per second


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }


    private Vector3 getRandomSpawnPosition()
    {
        var spawnXOffset = Random.Range(-1 * spawnHorizontalVariance, spawnHorizontalVariance);
        var spawnYOffset = Random.Range(-1 * spawnVerticalVariance, spawnVerticalVariance);
        return spawnPoint + new Vector3(spawnXOffset, spawnYOffset, 0);
    }

    IEnumerator SpawnRoutine()
    {
        // Calculate the delay between spawns
        float delay = 1f / spawnRate;

        while (true)
        {
            Instantiate(prefab, getRandomSpawnPosition(), new Quaternion(0,0,0,0));

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
