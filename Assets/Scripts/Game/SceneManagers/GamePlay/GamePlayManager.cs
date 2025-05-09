using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePlayManager
{
    [SerializeField] private PlatformsSpawnManager _platformsSpawnManager;
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    private Camera _mainCamera;
    private GameConfig _gameConfig;
   
    private GameObject _player;

    private Coroutine _spawnPlatforms;
    private Coroutine _objectGarbageCollector;

    public bool IsGameStarted { get; private set; }

    public void Init(GameConfig gameConfig, GameObject player)
    {
        _player = player;

        _gameConfig = gameConfig;

        _mainCamera = Camera.main;

        _platformsSpawnManager.Init(player.transform, _mainCamera, _gameConfig);
        _enemySpawnManager.Init(player.transform, _mainCamera, _gameConfig);
    }

    public void DeInit()
    {
        CoroutineServices.instance.StopRoutine(_spawnPlatforms);
        CoroutineServices.instance.StopRoutine(_objectGarbageCollector);
    }

    public void StartGame()
    {
        IsGameStarted = true;

        
        FirstSpawn();

         _spawnPlatforms = CoroutineServices.instance.StartRoutine(SpawnSceneObjects());
        _objectGarbageCollector = CoroutineServices.instance.StartRoutine(ObjectGarbageCollector());
    }

    private void FirstSpawn()
    {
        for (int i = 0; i < 5; i++)
        {
            List<PlatformTypes> platformsInRom = new List<PlatformTypes> { PlatformTypes.broken, PlatformTypes.movable, PlatformTypes.defaultPlatform };
            _platformsSpawnManager.SpawnPlatforms(platformsInRom);

            GameObject platform = _platformsSpawnManager.GetPlatformInRow(_platformsSpawnManager.PrewSpawnY);
            float xPos = platform.transform.position.x;
            _enemySpawnManager.SpawnEnemy(EnemyTypes.movableEnemy, _platformsSpawnManager.PrewSpawnY + 1f, xPos);
        }
    }

    private IEnumerator SpawnSceneObjects()
    {
        while (true)
        {
            List<PlatformTypes> platformsInRom = new List<PlatformTypes> { PlatformTypes.broken };
            _platformsSpawnManager.SpawnPlatforms(platformsInRom);
            yield return new WaitForSeconds(_gameConfig.SpawnIntervalTimer);
        }
    }
    private IEnumerator ObjectGarbageCollector()
    {
        while (true)
        {
            _platformsSpawnManager.ClearPlatformsGarbage(_player.transform);
            _enemySpawnManager.ClearEnemiesGarbage(_player.transform);

            yield return new WaitForSeconds(_gameConfig.ObjectGarbageCollectorInterval);
        }
    }
}