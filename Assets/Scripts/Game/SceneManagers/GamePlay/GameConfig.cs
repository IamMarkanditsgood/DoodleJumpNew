using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/GamePlayManager/Game/Level", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Configs")]
    [SerializeField] private CharacterPlayerConfig _playerConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [Tooltip("All used configs. You have to add here at list one config of each type of platform config")]
    [SerializeField] private BasicPlatformConfig[] _platformConfigs;
    [Tooltip("All used configs. You have to add here at list one config of each type of enemy config")]
    [SerializeField] private BasicEnemyConfig[] _enemyConfigs;
    [Tooltip("All used configs. You have to add here at list one config of each type of ammo config")]
    [SerializeField] private BasicAmmoConfig[] _ammoConfigs;

    [Header("Prefab References")]
    [Tooltip("Platforms prefab to spawn")]
    [SerializeField] private BasicPlatformController[] _platformPrefabs;
    [Tooltip("Enemies prefab to spawn")]
    [SerializeField] private BasicEnemyController[] _enemyPrefabs;
    [Tooltip("Ammo prefab to spawn")]
    [SerializeField] private BasicAmmoController[] _ammoPrefabs;
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
    [Tooltip("Vertical distance between platform rows")]
    [SerializeField] private float _maxRowSpacing = 2f;
    [Tooltip("Vertical distance between platform rows")]
    [SerializeField] private float _minRowSpacing = 0.5f;
    [Tooltip("Minimum horizontal distance between ammunitionsPool in same row")]
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


    public CharacterPlayerConfig PlayerConfig => _playerConfig;
    public CameraConfig CameraConfig => _cameraConfig;
    public BasicPlatformConfig[] PlatformConfigs => _platformConfigs;
    public BasicEnemyConfig[] EnemyConfigs => _enemyConfigs;
    public BasicAmmoConfig[] AmmoConfig => _ammoConfigs;
    public BasicPlatformController[] PlatformPrefabs => _platformPrefabs;
    public BasicEnemyController[] EnemyPrefabs => _enemyPrefabs;
    public BasicAmmoController[] AmmoPrefabs => _ammoPrefabs;
    public GameObject CameraPrefab => _cameraPrefab;
    public GameObject PlayerPref => _playerPref;
    public float ObjectGarbageCollectorInterval => _objectGarbageCollectorInterval;
    public float ObjectCleanupDistanceToPlayer => _objectCleanupDistanceToPlayer;
    public float SpawnIntervalTimer => _spawnIntervalTimer;
    public float MaxRowSpacing => _maxRowSpacing;
    public float MinRowSpacing => _minRowSpacing;
    public float MinDistanceBetweenPlatformsX => _minDistanceBetweenPlatformsX;
    public float MaxSpawnHeightAbovePlayer => _maxSpawnHeightAbovePlayer;
    public bool UseManualXBounds => _useManualXBounds;
    public float ManualMinX => _manualMinX;
    public float ManualMaxX => _manualMaxX;
}
