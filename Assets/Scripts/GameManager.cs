using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private int _currentLevel = 0;
    public List<Level> _levels = new List<Level>()
    {
        new Level(0, 45),
        new Level(30, 45),
        new Level(60, 45),
        new Level(90, 45)
    };

    public GameObject player;
    public GameObject mainCamera;
    public GameObject FirstLevelSpawners;
    private bool levelSwitchCooldown = false;

    // Start is called before the first frame update
    void Start() 
    {
        var pos = FirstLevelSpawners.transform.position;
        FirstLevelSpawners.transform.position = new Vector3(pos.x, _levels[0].height, _levels[0].z);
        FirstLevelSpawners.GetComponents<ObjectSpitter>()[0].isSpawning = true;
        FirstLevelSpawners.GetComponents<ObjectSpitter>()[1].isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelSwitchCooldown) return;

        var playerFellDownLevel = player.transform.position.y < _levels[_currentLevel].height - 2 && !player.GetComponent<PropelToNextLevel>().isPropelling && player.GetComponent<Rigidbody>().velocity.y < 0;
        if (playerFellDownLevel && _currentLevel > 0)
        {
            Debug.Log($"Player fell down level. {player.transform.position.y}, {player.GetComponent<PropelToNextLevel>().isPropelling}");
            SetCurrentLevelDown(_currentLevel - 1);
        }
    }

    public void TrampolineHit()
    {
        if (levelSwitchCooldown) return;

        SetCurrentLevelUp(_currentLevel + 1);
    }

    private void SetCurrentLevelUp(int newLevel)
    {
        Debug.Log("moved UP to level " + newLevel);
        var nextLevel = _levels[newLevel];
        player.GetComponent<PropelToNextLevel>().PropelTo(nextLevel.height + 10);
        mainCamera.GetComponent<MoveCamera>().MoveToHeight(nextLevel.height + 6);
        _currentLevel = newLevel;

        levelSwitchCooldown = true;
        StartCoroutine(LevelAdvanceCooldown(.5f));
    }


    // this breaks ascending camera for some reason
    private void SetCurrentLevelDown(int newLevel)
    {
        Debug.Log("moved DOWN to level " + newLevel);
        var nextLevel = _levels[newLevel];
        mainCamera.GetComponent<MoveCamera>().MoveToHeight(nextLevel.height + 6);
        player.transform.position = new Vector3(player.transform.position.x, nextLevel.height + 10, player.transform.position.z);
        player.GetComponent<BufferFallSpeed>().BufferUntilHeight(nextLevel.height + 7);
        _currentLevel = newLevel;

        levelSwitchCooldown = true;
        StartCoroutine(LevelAdvanceCooldown(.5f));
    }

    // Coroutine to handle the cooldown
    IEnumerator LevelAdvanceCooldown(float cooldownDuration)
    {
        yield return new WaitForSeconds(cooldownDuration);
        levelSwitchCooldown = false;
    }
}


public class Level
{
    public readonly float height;
    public readonly float z;

    public Level(float height, float z)
    {
        this.height = height;
        this.z = z;
    }
}