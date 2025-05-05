using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPlatformController : MonoBehaviour
{
    [SerializeField] private PlatformTypes _platformType;

    protected BasicPlatformConfig _basicPlatformConfig;

    public PlatformTypes PlatformType => _platformType;

    public virtual void Init(BasicPlatformConfig basicPlatformConfig)
    {
        _basicPlatformConfig = basicPlatformConfig;
    }
}
