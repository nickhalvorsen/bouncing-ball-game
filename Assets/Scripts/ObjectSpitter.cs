using System.Collections;
using UnityEngine;

public class ObjectSpitter : MonoBehaviour
{
    public GameObject prefabToSpit;
    public float yOffset; // horizontal spawn point
    public float zOffset;
    public float spawnHorizontalVariance = 20f;
    public float spawnVerticalVariance = .5f;
    public float spawnRate = 2f; // Number of spawns per second
    public bool isSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPrefabRoutine());
    }


    private Vector3 getRandomSpawnPosition(float y, float z)
    {
        var spawnPoint = new Vector3(0, y, z);
        var spawnXRandomOffset = Random.Range(-1 * spawnHorizontalVariance, spawnHorizontalVariance);
        var spawnYRandomOffset = Random.Range(-1 * spawnVerticalVariance, spawnVerticalVariance);
        return spawnPoint + new Vector3(spawnXRandomOffset, spawnYRandomOffset, 0);
    }

    IEnumerator SpawnPrefabRoutine()
    {
        // Calculate the delay between spawns
        float delay = 1f / spawnRate;

        while (true)
        {
            var position = transform.position;
            if (isSpawning)
                Instantiate(prefabToSpit, getRandomSpawnPosition(position.y + yOffset, position.z + zOffset), new Quaternion(0, 0, 0, 0));

            yield return new WaitForSeconds(delay);
        }
    }

    // Used to spawn platforms at the beginning of the game
    public void SpawnToFillArea()
    {
        var objectSpeed = 5f;
        var distanceCovered = 0f;

        var endZ = transform.position.z + zOffset;
        var position = transform.position;

        while (distanceCovered < endZ)
        {
            Instantiate(prefabToSpit, getRandomSpawnPosition(position.y + yOffset, endZ - distanceCovered), new Quaternion(0, 0, 0, 0));
            distanceCovered += objectSpeed * (1 / spawnRate);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
