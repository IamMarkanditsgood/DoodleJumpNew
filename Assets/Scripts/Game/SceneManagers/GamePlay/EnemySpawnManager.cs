using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnManager
{
    private GameConfig _gameConfig;
    private Camera _mainCamera;
    private Transform _player;
    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    private float _minXSpawnPos;
    private float _maxXSpawnPos;

    public void Init(Transform player, Camera mainCamera, GameConfig gameConfig)
    {
        _player = player;
        _mainCamera = mainCamera;
        _gameConfig = gameConfig;
        SetSpawnBounds();
    }

    public void ClearEnemiesGarbage(Transform player)
    {
        List<GameObject> enemiesToRemove = new List<GameObject>();

        foreach (GameObject enemy in _enemies)
        {
            bool isBelowPlayer = enemy.transform.position.y < player.position.y;

            if (isBelowPlayer && Vector2.Distance(player.position, enemy.transform.position) > _gameConfig.ObjectCleanupDistanceToPlayer)
            {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach (GameObject enemy in enemiesToRemove)
        {
            BasicEnemyController enemyController = enemy.GetComponent<BasicEnemyController>();
            PoolObjectManager.instant.enemyPoolObjectManager.DisableEnemy(enemyController, enemyController.EnemyType);
        }
    }

    public void SpawnEnemy(EnemyTypes enemyType, float yPos, float? xPos = null)
    {
        foreach(var enemy in _enemies)
        {
            if (enemy.transform.position.y == yPos)
            {
                return;
            }
        }

        float spawnX = xPos ?? UnityEngine.Random.Range(_minXSpawnPos, _maxXSpawnPos);
        Vector2 spawnPos = new Vector2(spawnX, yPos);

        SetEnemyOnPos(spawnPos, enemyType);
    }

    private void SetEnemyOnPos(Vector2 pos, EnemyTypes enemyType)
    {
        BasicEnemyController newEnemy = PoolObjectManager.instant.enemyPoolObjectManager.GetEnemy(enemyType);
        newEnemy.Init(GetConfigByType(enemyType));
        newEnemy.Toggle(true);
        newEnemy.transform.position = pos;
        _enemies.Add(newEnemy.gameObject);
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

    private BasicEnemyConfig GetConfigByType(EnemyTypes type)
    {
        foreach (var enemyConfig in _gameConfig.EnemyConfigs)
        {
            if (enemyConfig.EnemyType == type)
            {
                return enemyConfig;
            }
        }
        Debug.LogError($"You do not have {type} enemy config");
        return null;
    }
}