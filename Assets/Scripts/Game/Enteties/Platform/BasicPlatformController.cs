using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPlatformController : MonoBehaviour
{
    [SerializeField] private PlatformTypes _platformType;

    public PlatformTypes PlatformType => _platformType;
}
