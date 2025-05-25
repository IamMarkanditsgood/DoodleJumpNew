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

    private float _prevSpawnY;
    private float _nextSpawnY;
    private float _minXSpawnPos;
    private float _maxXSpawnPos;

    public float PrewSpawnY => _prevSpawnY;
    public float NextSpawnY => _nextSpawnY;

    public void Init(Transform player, Camera mainCamera, GameConfig gameConfig)
    {
        _player = player;
        _mainCamera = mainCamera;
        _gameConfig = gameConfig;
        _prevSpawnY = _startSpawnYPos.position.y;
        _nextSpawnY = _startSpawnYPos.position.y;
        SetSpawnBounds();
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
            platformController.Toggle(false);
            PoolObjectManager.instant.platformsPoolObjectManager.DisablePlatform(platformController, platformController.PlatformType);
        }
    }

    public GameObject GetPlatformInRow(float yRow)
    {
        foreach (var platform in _spawnedPlatforms)
        {
            if (platform == null) continue;

            if (Mathf.Approximately(platform.transform.position.y, yRow))
            {
                return platform;
            }
        }

        Debug.LogWarning($"No platform found in row: {yRow}");
        return null;
    }

    public void SpawnPlatforms(List<PlatformTypes> platformType, bool spawnRandomBooster = false)
    {
        if (!IsSpawnAvailable()) return;

        bool spanwBuster = spawnRandomBooster;

        for (int i = 0; i < platformType.Count; i++)
        {
            SpawnPlatform(platformType[i], spanwBuster);
            spanwBuster = false;
        }

        float nextYRowPos = UnityEngine.Random.Range(_gameConfig.MinRowSpacing, _gameConfig.MaxRowSpacing);

        _prevSpawnY = _nextSpawnY;
        _nextSpawnY += nextYRowPos;
    }

    private void SpawnPlatform(PlatformTypes platformType, bool spawnRandomBooster = false)
    {
        float spawnX;
        bool positionIsValid;
        int attempts = 0;

        do
        {
            spawnX = UnityEngine.Random.Range(_minXSpawnPos, _maxXSpawnPos);
            positionIsValid = IsPositionValid(spawnX);
            attempts++;

        } while (!positionIsValid && attempts <= 100);

        if (positionIsValid)
        {
            SpawnPlatformOnPosition(spawnX, platformType, spawnRandomBooster);
        }
    }

    public bool IsSpawnAvailable()
    {
        float playerY = _player.transform.position.y;
        return (_nextSpawnY - playerY) < _gameConfig.MaxSpawnHeightAbovePlayer;
    }

    private void SetSpawnBounds()
    {
        if (_gameConfig.UseManualXBounds)
        {
            _minXSpawnPos = _gameConfig.ManualMinX;
            _maxXSpawnPos = _gameConfig.ManualMaxX;
        }
        else
        {
            float cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            Vector3 cameraPos = _mainCamera.transform.position;
            _minXSpawnPos = cameraPos.x - cameraHalfWidth;
            _maxXSpawnPos = cameraPos.x + cameraHalfWidth;
        }
    }

    private bool IsPositionValid(float xPos)
    {
        for (int i = 0; i < _spawnedPlatforms.Count; i++)
        {
            Vector3 platformPos = _spawnedPlatforms[i].transform.position;

            if (Mathf.Approximately(platformPos.y, _nextSpawnY))
            {
                if (Mathf.Abs(platformPos.x - xPos) < _gameConfig.MinDistanceBetweenPlatformsX)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void SpawnPlatformOnPosition(float spanwPos, PlatformTypes platformType, bool spawnRandomBooster = false)
    {
        Vector2 spawnPos = new Vector2(spanwPos, _nextSpawnY);

        BasicPlatformController platform = PoolObjectManager.instant.platformsPoolObjectManager.GetPlatform(platformType);
        platform.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);

        _spawnedPlatforms.Add(platform.gameObject);

        platform.Init(GetConfigByType(platformType));
        platform.Toggle(true);
        if (spawnRandomBooster)
            platform.AddRandomBuster();
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