using System.Collections;
using UnityEngine;

public class PlatformSpitter : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject trampolinePrefab;
    public float y; // horizontal spawn point
    public float spawnHorizontalVariance = 20f;
    public float spawnVerticalVariance = 0.2f;
    public float spawnRate = 2f; // Number of spawns per second
    public bool spawnTrampoline = false;
    private float trampolineSpawnSecondsBetween = 4; 


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPlatformRoutine());
        StartCoroutine(SpawnTrampolineRoutine());
    }


    private Vector3 getRandomPlatformSpawnPosition()
    {
        var spawnPoint = new Vector3(0, y, 15);
        var spawnXOffset = UnityEngine.Random.Range(-1 * spawnHorizontalVariance, spawnHorizontalVariance);
        var spawnYOffset = UnityEngine.Random.Range(-1 * spawnVerticalVariance, spawnVerticalVariance);
        return spawnPoint + new Vector3(spawnXOffset, spawnYOffset, 0);
    }
    private Vector3 getRandomTrampolineSpawnPosition()
    {
        // Need a different z offset to get the trampolines to generate at the same z position as platforms.
        // I have no clue why this is necessary. 
        var spawnPoint = new Vector3(0, y, 50);
        var spawnXOffset = UnityEngine.Random.Range(-1 * spawnHorizontalVariance, spawnHorizontalVariance);
        var spawnYOffset = UnityEngine.Random.Range(-2 * spawnVerticalVariance, 2*spawnVerticalVariance);
        return spawnPoint + new Vector3(spawnXOffset, spawnYOffset, 0);
    }

    IEnumerator SpawnPlatformRoutine()
    {
        // Calculate the delay between spawns
        float delay = 1f / spawnRate;

        while (true)
        {
            Instantiate(platformPrefab, getRandomPlatformSpawnPosition(), new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SpawnTrampolineRoutine()
    {

        while (true)
        {
            Instantiate(trampolinePrefab, getRandomTrampolineSpawnPosition(), new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(trampolineSpawnSecondsBetween);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
