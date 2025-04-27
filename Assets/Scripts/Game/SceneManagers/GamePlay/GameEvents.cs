using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnPlatformSpawned;

    public static void SpawnPlatform(GameObject platform) => OnPlatformSpawned?.Invoke(platform);
}
