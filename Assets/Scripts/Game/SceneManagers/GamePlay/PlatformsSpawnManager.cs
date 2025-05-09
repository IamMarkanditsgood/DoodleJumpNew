using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlatformsSpawnManager
{
    [SerializeField] private Transform _startSpawnYPos;
    
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
        _nextSpawnY = _startSpawnYPos.position.y;
    }

    public void ClearPlatformsGarbage(Transform player)
    {
        List<GameObject> platformsToRemove = new List<GameObject>();

        foreach (GameObject platform in _spawnedPlatforms)
        {
            bool isBelowPlayer = platform.transform.position.y < player.position.y;

            if (isBelowPlayer && Vector2.Distance(player.position, platform.transform.position) > _gameConfig.ObjectCleanupDistanceToPlayer)
            {
                platformsToRemove.Add(platform);
            }
        }

        foreach (GameObject platform in platformsToRemove)
        {
            _spawnedPlatforms.Remove(platform);

            BasicPlatformController platformController = platform.GetComponent<BasicPlatformController>();
            PoolObjectManager.instant.platformsPoolObjectManager.DisablePlatform(platformController, platformController.PlatformType);
        }
    }

    public void SpawnPlatformRow(int platformsAmmount, PlatformTypes platformType)
    {
        if (ShouldSkipSpawn()) return;

        GetSpawnBounds(out float minX, out float maxX);
        SpawnPlatformsInRow(minX, maxX, platformsAmmount, platformType);

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

    private void SpawnPlatformsInRow(float minX, float maxX, int platformsAmmount, PlatformTypes platformType)
    {
        List<float> platformPositionsX = new List<float>();

        for (int i = 0; i < platformsAmmount; i++)
        {
            TrySpawnPlatform(minX, maxX, platformPositionsX, i, platformsAmmount, platformType);
        }
    }

    private void TrySpawnPlatform(float minX, float maxX, List<float> existingPositions, int platformIndex, int platformsAmmount, PlatformTypes platformType)
    {
        float spawnX;
        bool positionIsValid;
        int attempts = 0;

        do
        {
            spawnX = CalculatePlatformX(minX, maxX, platformIndex, platformsAmmount);
            positionIsValid = IsPositionValid(spawnX, existingPositions);
            attempts++;

        } while (!positionIsValid && attempts <= 100);

        if (positionIsValid)
        {
            SpawnSinglePlatform(spawnX, platformType);
            existingPositions.Add(spawnX);
        }
    }

    private float CalculatePlatformX(float minX, float maxX, int platformIndex, int platformsAmmount)
    {
        float step = (maxX - minX) / (platformsAmmount + 1);
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

    private void SpawnSinglePlatform(float xPos, PlatformTypes platformType)
    {
        Vector2 spawnPos = new Vector2(xPos, _nextSpawnY);

        BasicPlatformController platform = PoolObjectManager.instant.platformsPoolObjectManager.GetPlatform(platformType);
        platform.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);

        _spawnedPlatforms.Add(platform.gameObject);

        platform.Init(GetConfigByType(platformType));


        if (platform is MovablePlatform)
        {
            SetMovablePlatform((MovablePlatform)platform);
        }
    }

    private void SetMovablePlatform(MovablePlatform movablePlatform)
    {
        movablePlatform.SetSpeedValue(5);
    }

    private BasicPlatformConfig GetConfigByType(PlatformTypes type)
    {
        foreach(var platformConfig in _gameConfig.PlatformConfigs)
        {
            if(platformConfig.PlatformType == type)
            {
                return platformConfig;
            }
        }
        Debug.LogError($"You do not have {type} platform config");
        return null;
    }
}