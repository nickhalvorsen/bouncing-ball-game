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


    private Vector3 getRandomSpawnPosition()
    {
        var position = transform.position;
        var spawnPoint = new Vector3(0, position.y + yOffset, position.z + zOffset);
        var spawnXOffset = Random.Range(-1 * spawnHorizontalVariance, spawnHorizontalVariance);
        var spawnYOffset = Random.Range(-1 * spawnVerticalVariance, spawnVerticalVariance);
        return spawnPoint + new Vector3(spawnXOffset, spawnYOffset, 0);
    }

    IEnumerator SpawnPrefabRoutine()
    {
        // Calculate the delay between spawns
        float delay = 1f / spawnRate;

        while (true)
        {
            if (isSpawning)
                Instantiate(prefabToSpit, getRandomSpawnPosition(), new Quaternion(0, 0, 0, 0));

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
