using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PoolObjectManager
{
    [SerializeField] private PlatformsPoolObjectManager _platformsPoolObjectManager;
    [SerializeField] private EnemyPoolObjectManager _enemyPoolObjectManager;

    public PlatformsPoolObjectManager platformsPoolObjectManager => _platformsPoolObjectManager;
    public EnemyPoolObjectManager enemyPoolObjectManager => _enemyPoolObjectManager;

    public static PoolObjectManager instant;

    private PoolObjectManagerInitDataKit _poolObjectInitKit;

    public void Init(PoolObjectManagerInitDataKit poolObjectInitKit)
    {
        if (instant == null)
        {
            instant = this;
        }
        _poolObjectInitKit = poolObjectInitKit;
        InitPoolObjects();
    }

    public void DeInit()
    {
        if (instant != null)
        {
            instant = null;
        }
    }

    private void InitPoolObjects()
    {
        _platformsPoolObjectManager.InitPools(_poolObjectInitKit.platforms);
        _enemyPoolObjectManager.InitPools(_poolObjectInitKit.enemies);
    }
}
