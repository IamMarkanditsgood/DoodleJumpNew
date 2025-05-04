using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/GamePlayManager/Game/Level", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Configs")]
    [SerializeField] private CharacterPlayerConfig _playerConfig;
    [SerializeField] private CameraConfig _cameraConfig;

    [Header("Prefab References")]
    [Tooltip("Platform prefab to spawn")]
    [SerializeField] private BasicPlatformController[] _platformPrefabs;
    [Tooltip("Camera prefab reference")]
    [SerializeField] private GameObject _cameraPrefab;
    [Tooltip("Player character prefab")]
    [SerializeField] private GameObject _playerPref;

    [Header("Global game parameters")]
    [Tooltip("Time interval between object garbage collector that clear objects that are to far and not in use. Interval in seconds")]
    [SerializeField] private float _objectGarbageCollectorInterval = 30f;
    [Tooltip("Distance from the player at which objects are removed")]
    [SerializeField] private float _objectCleanupDistanceToPlayer = 30f;  

    [Header("Platform Spawner Settings")]

    [Header("Spawn Timing")]
    [Tooltip("Time interval between platform row spawns (seconds)")]
    [SerializeField] private float _spawnIntervalTimer = 1f;

    [Header("Platform Row Configuration")]
    [Tooltip("Number of platforms per horizontal row")]
    [SerializeField] private int _maxPlatformsPerRow = 3;
    [Tooltip("Vertical distance between platform rows")]
    [SerializeField] private float _rowSpacing = 2f;
    [Tooltip("Minimum horizontal distance between platforms in same row")]
    [SerializeField] private float _minDistanceBetweenPlatformsX = 1.2f;
    [Tooltip("Maximum spawn height above player position")]
    [SerializeField] private float _maxSpawnHeightAbovePlayer = 10;

    [Header("Horizontal Spawn Boundaries")]
    [Tooltip("Enable to set custom X spawn boundaries")]
    [SerializeField] private bool _useManualXBounds = false;
    [Tooltip("Minimum X position when using manual bounds")]
    [SerializeField] private float _manualMinX = -2f;
    [Tooltip("Maximum X position when using manual bounds")]
    [SerializeField] private float _manualMaxX = 2f;

    [Header("Platform Scattering")]
    [Tooltip("Random horizontal offset range for platform positions")]
    [Range(0, 2)]
    [SerializeField] private float _xSpacing = 1.5f;

    public CharacterPlayerConfig PlayerConfig => _playerConfig;
    public CameraConfig CameraConfig => _cameraConfig;
    public BasicPlatformController[] PlatformPrefabs => _platformPrefabs;
    public GameObject CameraPrefab => _cameraPrefab;
    public GameObject PlayerPref => _playerPref;
    public float ObjectGarbageCollectorInterval => _objectGarbageCollectorInterval;
    public float ObjectCleanupDistanceToPlayer => _objectCleanupDistanceToPlayer;
    public float SpawnIntervalTimer => _spawnIntervalTimer;
    public int MaxPlatformsPerRow => _maxPlatformsPerRow;
    public float RowSpacing => _rowSpacing;
    public float MinDistanceBetweenPlatformsX => _minDistanceBetweenPlatformsX;
    public float MaxSpawnHeightAbovePlayer => _maxSpawnHeightAbovePlayer;
    public float XSpacing => _xSpacing;
    public bool UseManualXBounds => _useManualXBounds;
    public float ManualMinX => _manualMinX;
    public float ManualMaxX => _manualMaxX;
}
