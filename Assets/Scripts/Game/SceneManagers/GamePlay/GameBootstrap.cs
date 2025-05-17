using UnityEngine;

/// <summary>
/// The main initializer of the game. Starts all systems in the correct order.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private PoolObjectManager _poolObjectManager;
    [SerializeField] private SceneBuilder _sceneBuilder;
    [SerializeField] private GamePlayManager _gamePlayManager;

    private SceneConstructionKit _sceneConstructionKit = new SceneConstructionKit();

    private void Awake()
    {
        ValidateDependencies();
        InitializeSystems();
    }

    private void Start()
    {
        BuildScene();
        StartGame();
    }

    private void OnDestroy()
    {
        _gamePlayManager.DeInit();
    }

    private void BuildScene()
    {
        _sceneBuilder.BuildScene();
        _sceneBuilder.Player.GetComponent<CharacterPlayerController>().Init(_gameConfig.PlayerConfig);
    }

    private void StartGame()
    {
        _gamePlayManager.Init(_gameConfig, _sceneBuilder.Player);
        _gamePlayManager.StartGame();
    }

    private void ValidateDependencies()
    {
        if (_gameConfig == null)
            Debug.LogError($"{nameof(GameConfig)} is not assigned!", this);

        if (_poolObjectManager == null)
            Debug.LogError($"{nameof(PoolObjectManager)} is not assigned!", this);

        if (_sceneBuilder == null)
            Debug.LogError($"{nameof(SceneBuilder)} is not assigned!", this);

        if (_gamePlayManager == null)
            Debug.LogError($"{nameof(GamePlayManager)} is not assigned!", this);
    }

    private void InitializeSystems()
    {
        InitPoolObjectManger();
        InitSceneBuilderSystem();
    }

    private void InitPoolObjectManger()
    {
        PoolObjectManagerInitDataKit poolObjectManagerInitDataKit = new PoolObjectManagerInitDataKit();

        poolObjectManagerInitDataKit.platforms = _gameConfig.PlatformPrefabs;
        poolObjectManagerInitDataKit.enemies = _gameConfig.EnemyPrefabs;
        poolObjectManagerInitDataKit.ammunitions = _gameConfig.AmmoPrefabs;

        _poolObjectManager.Init(poolObjectManagerInitDataKit);
    }

    private void InitSceneBuilderSystem()
    {
        SetConstuctionKit();
        _sceneBuilder.Init(_sceneConstructionKit);
    }

    private void SetConstuctionKit()
    {
        _sceneConstructionKit.cameraPrefab = _gameConfig.CameraPrefab;
        _sceneConstructionKit.playerPrefab = _gameConfig.PlayerPref;
        _sceneConstructionKit.playerConfig = _gameConfig.PlayerConfig;
        _sceneConstructionKit.cameraConfig = _gameConfig.CameraConfig;
    }
}
