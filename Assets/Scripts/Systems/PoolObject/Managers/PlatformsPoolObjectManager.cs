using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlatformsPoolObjectManager
{
    [Header("Containers")]
    [SerializeField] private Transform _platformsContainer;

    private List<PlatformData> _platformPoolObjects = new();

    public class PlatformData
    {    
        public ObjectPool<BasicPlatformController> platforms = new ObjectPool<BasicPlatformController>();
        public PlatformTypes Type;
    }

    public void InitPools(BasicPlatformController[] platforms)
    {
        foreach (var platform in platforms)
        {
            PlatformData newPlatformData = new PlatformData();
            newPlatformData.Type = platform.PlatformType;
            newPlatformData.platforms.InitializePool(platform, _platformsContainer, 30);

            _platformPoolObjects.Add(newPlatformData);
        }
    }

    public BasicPlatformController GetPlatform(PlatformTypes type)
    {
        foreach (var pool in _platformPoolObjects)
        {
            if (pool.Type == type)
            {
                return pool.platforms.GetFreeComponent();
            }
        }

        Debug.LogError($"There is no {type} type of platform in pool!");
        return null;
    }

    public void DisablePlatform(BasicPlatformController platform, PlatformTypes type)
    {
        foreach (var pool in _platformPoolObjects)
        {
            if (pool.Type == type)
            {
                pool.platforms.DisableComponent(platform);
                return;
            }
        }

        Debug.LogError($"There is no {type} type of platform in pool!");
    }
}