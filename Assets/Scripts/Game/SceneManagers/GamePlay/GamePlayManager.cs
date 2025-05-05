using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[Serializable]
public class GamePlayManager
{
    [SerializeField] private PlatformsManager _platformsManager;

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

        _platformsManager.Init(player.transform, _mainCamera, _gameConfig);
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
            _platformsManager.SpawnPlatformRow(_gameConfig.MaxPlatformsPerRow, PlatformTypes.broken);
        }
    }

    private IEnumerator SpawnSceneObjects()
    {
        while (true)
        {
            _platformsManager.SpawnPlatformRow(_gameConfig.MaxPlatformsPerRow, PlatformTypes.broken);
            yield return new WaitForSeconds(_gameConfig.SpawnIntervalTimer);
        }
    }
    private IEnumerator ObjectGarbageCollector()
    {
        while (true)
        {
            _platformsManager.ClearPlatformsGarbage(_player.transform);
            yield return new WaitForSeconds(_gameConfig.ObjectGarbageCollectorInterval);
        }
    }
}