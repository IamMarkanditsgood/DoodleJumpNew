using UnityEngine;

public class BasicPlatformConfig : ScriptableObject
{
    [SerializeField] private PlatformTypes _platformType;

    public PlatformTypes PlatformType => _platformType;
}