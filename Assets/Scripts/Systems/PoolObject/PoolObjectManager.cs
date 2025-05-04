using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PoolObjectManager
{
    [SerializeField] private PlatformsPoolObjectManager _platformsPoolObjectManager;

    public PlatformsPoolObjectManager platformsPoolObjectManager => _platformsPoolObjectManager;

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
    }
}
