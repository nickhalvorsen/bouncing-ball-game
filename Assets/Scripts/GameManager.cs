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
        new Level(0, 60),
        new Level(30, 80)
    };

    public GameObject player;
    public GameObject mainCamera;
    public GameObject PlatformSpitter0;
    public GameObject PlatformSpitter1;

    // Start is called before the first frame update
    void Start()
    {
        PlatformSpitter0.GetComponent<PlatformSpitter>().y = _levels[0].height;
        PlatformSpitter1.GetComponent<PlatformSpitter>().y = _levels[1].height;
    }

    // Update is called once per frame
    void Update()
    {
        var playerFellDownLevel = player.transform.position.y < _levels[_currentLevel].height - 2 && !player.GetComponent<PropelToNextLevel>().isPropelling;
        if (playerFellDownLevel && _currentLevel > 0)
        {
            Debug.Log($"Player fell down level. {player.transform.position.y}, {player.GetComponent<PropelToNextLevel>().isPropelling}");
            SetCurrentLevelDown(_currentLevel - 1);
        }
    }

    public void TrampolineHit()
    {
        // todo: prevent double hit from incrementing two levels
        SetCurrentLevelUp(_currentLevel + 1);
    }

    private void SetCurrentLevelUp(int newLevel)
    {
        Debug.Log("UP level" + newLevel);
        var nextLevel = _levels[newLevel];
        player.GetComponent<PropelToNextLevel>().PropelTo(nextLevel.height + 10);
        mainCamera.GetComponent<MoveCamera>().MoveToHeight(nextLevel.height + 9);
        _currentLevel = newLevel;
    }


    // this breaks ascending camera for some reason
    private void SetCurrentLevelDown(int newLevel)
    {
        Debug.Log("Down level" + newLevel);
        var nextLevel = _levels[newLevel];
        mainCamera.GetComponent<MoveCamera>().MoveToHeight(nextLevel.height + 9);
        player.transform.position = new Vector3(player.transform.position.x, nextLevel.height + 12, player.transform.position.z);
        player.GetComponent<BufferFallSpeed>().BufferUntilHeight(nextLevel.height + 9);
        _currentLevel = newLevel;
    }
}


public class Level
{
    public readonly float height;
    public readonly float secondsToComplete;
    public bool completed = false;
    public float timeSpent = 0f;

    public Level(float height, float secondsToComplete)
    {
        this.height = height;
        this.secondsToComplete = secondsToComplete;
    }
}