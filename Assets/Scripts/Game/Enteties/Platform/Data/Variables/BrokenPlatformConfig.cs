using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BrokenPlatform", menuName = "ScriptableObjects/GamePlayManager/Enteties/Platforms/BrokenPlatform", order = 1)]
public class BrokenPlatformConfig : BasicPlatformConfig
{
    [Tooltip("Delay before platform destruction")]
    [SerializeField] private float _destroyDelay;

    public float DestroyDelay => _destroyDelay;
}
