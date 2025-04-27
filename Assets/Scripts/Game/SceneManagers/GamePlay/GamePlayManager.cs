using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePlayManager
{
    private Camera _mainCamera;
    private GameConfig _gameConfig;
    private PlatformSpawner _platformSpawner = new PlatformSpawner();

    public bool IsGameStarted { get; private set; }

    public void Init(GameConfig gameConfig, Transform player)
    {
        _gameConfig = gameConfig;

        _mainCamera = Camera.main;

        _platformSpawner.Init(player, _mainCamera, _gameConfig);
    }

    public void StartGame()
    {
        IsGameStarted = true;
        CoroutineServices.instance.StartRoutine(SpawnSceneObjects());
    }

    public IEnumerator SpawnSceneObjects()
    {
        while (true)
        {
            _platformSpawner.SpawnPlatformRow();
            yield return new WaitForSeconds(_gameConfig.SpawnIntervalTimer);
        }
    }
}

public class PlatformSpawner
{
    private GameConfig _gameConfig;
    private Camera _mainCamera;
    private Transform _player;
    private List<GameObject> _spawnedPlatforms = new List<GameObject>();
    private float _nextSpawnY; 

    public void Init(Transform player, Camera mainCamera, GameConfig gameConfig)
    {
        _player = player;
        _mainCamera = mainCamera;
        _gameConfig = gameConfig;
        _nextSpawnY = _mainCamera.transform.position.y;
    }

    public void SpawnPlatformRow()
    {
        if (ShouldSkipSpawn()) return;

        GetSpawnBounds(out float minX, out float maxX);
        SpawnPlatformsInRow(minX, maxX);

        _nextSpawnY += _gameConfig.RowSpacing;
    }

    private bool ShouldSkipSpawn()
    {
        float playerY = _player.transform.position.y;
        return _nextSpawnY - playerY > _gameConfig.MaxSpawnHeightAbovePlayer;
    }

    private void GetSpawnBounds(out float minX, out float maxX)
    {
        if (_gameConfig.UseManualXBounds)
        {
            minX = _gameConfig.ManualMinX;
            maxX = _gameConfig.ManualMaxX;
        }
        else
        {
            float cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            Vector3 cameraPos = _mainCamera.transform.position;
            minX = cameraPos.x - cameraHalfWidth;
            maxX = cameraPos.x + cameraHalfWidth;
        }
    }

    private void SpawnPlatformsInRow(float minX, float maxX)
    {
        List<float> platformPositionsX = new List<float>();

        for (int i = 0; i < _gameConfig.PlatformsPerRow; i++)
        {
            TrySpawnPlatform(minX, maxX, platformPositionsX, i);
        }
    }

    private void TrySpawnPlatform(float minX, float maxX, List<float> existingPositions, int platformIndex)
    {
        float spawnX;
        bool positionIsValid;
        int attempts = 0;

        do
        {
            spawnX = CalculatePlatformX(minX, maxX, platformIndex);
            positionIsValid = IsPositionValid(spawnX, existingPositions);
            attempts++;

        } while (!positionIsValid && attempts <= 100);

        if (positionIsValid)
        {
            SpawnSinglePlatform(spawnX, existingPositions);
        }
    }

    private float CalculatePlatformX(float minX, float maxX, int platformIndex)
    {
        float step = (maxX - minX) / (_gameConfig.PlatformsPerRow + 1);
        return minX + (platformIndex + 1) * step +
               UnityEngine.Random.Range(-_gameConfig.XSpacing, _gameConfig.XSpacing);
    }

    private bool IsPositionValid(float xPos, List<float> existingPositions)
    {
        foreach (float existingX in existingPositions)
        {
            if (Mathf.Abs(xPos - existingX) < _gameConfig.MinDistanceBetweenPlatformsX)
            {
                return false;
            }
        }
        return true;
    }

    private void SpawnSinglePlatform(float xPos, List<float> positionsList)
    {
        Vector2 spawnPos = new Vector2(xPos, _nextSpawnY);
        GameObject platform = UnityEngine.Object.Instantiate(_gameConfig.PlatformPrefab, spawnPos, Quaternion.identity);
        _spawnedPlatforms.Add(platform);
        positionsList.Add(xPos);
    }
}