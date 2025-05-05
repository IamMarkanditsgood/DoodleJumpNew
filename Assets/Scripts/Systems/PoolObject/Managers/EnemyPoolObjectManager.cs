using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyPoolObjectManager
{
    [Header("Containers")]
    [SerializeField] private Transform _enemyContainer;

    private List<EnemyData> _enemyPoolObjects = new();

    public class EnemyData
    {
        public ObjectPool<BasicEnemyController> enemies = new ObjectPool<BasicEnemyController>();
        public EnemyTypes Type;
    }

    public void InitPools(BasicEnemyController[] platforms)
    {
        foreach (var platform in platforms)
        {
            EnemyData newPlatformData = new EnemyData();
            newPlatformData.Type = platform.EnemyType;
            newPlatformData.enemies.InitializePool(platform, _enemyContainer, 30);

            _enemyPoolObjects.Add(newPlatformData);
        }
    }

    public BasicEnemyController GetEnemy(EnemyTypes type)
    {
        foreach (var pool in _enemyPoolObjects)
        {
            if (pool.Type == type)
            {
                return pool.enemies.GetFreeComponent();
            }
        }

        Debug.LogError($"There is no {type} type of platform in pool!");
        return null;
    }

    public void DisableEnemy(BasicEnemyController platform, EnemyTypes type)
    {
        foreach (var pool in _enemyPoolObjects)
        {
            if (pool.Type == type)
            {
                pool.enemies.DisableComponent(platform);
                return;
            }
        }

        Debug.LogError($"There is no {type} type of platform in pool!");
    }
}
