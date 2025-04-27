using System;
using UnityEngine;

/// <summary>
/// The main initializer of the game. Starts all systems in the correct order.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameConfig _gameConfig;
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

    private void BuildScene()
    {
        _sceneBuilder.BuildScene();
        _sceneBuilder.Player.GetComponent<CharacterPlayerController>().Init(_gameConfig.PlayerConfig);
    }

    private void StartGame()
    {
        _gamePlayManager.Init(_gameConfig, _sceneBuilder.Player.transform);
        _gamePlayManager.StartGame();
    }

    private void ValidateDependencies()
    {
        if (_gameConfig == null)
            Debug.LogError($"{nameof(GameConfig)} is not assigned!", this);

        if (_sceneBuilder == null)
            Debug.LogError($"{nameof(SceneBuilder)} is not assigned!", this);
    }

    private void InitializeSystems()
    {
        InitSceneBuilderSystem();
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

/// <summary>
/// Responsible for constructing the game scene: _player, environment, spawn points.
/// </summary>
[Serializable]
public class SceneBuilder
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform _defaultPlayerSpawnPoint;
    [SerializeField] private Transform _defaultCameraSpawnPoint;

    private SceneConstructionKit _sceneConstructionKit;

    public GameObject Player { get; private set; }

    public void Init(SceneConstructionKit sceneConstructionKit)
    {
        _sceneConstructionKit = sceneConstructionKit;
    }

    public void BuildScene()
    {
        SpawnPlayer();
        SpawnCamera();
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPosition = _defaultPlayerSpawnPoint != null ?
            _defaultPlayerSpawnPoint.position :
            Vector3.zero;

        Quaternion spawnRotation = _defaultPlayerSpawnPoint != null ?
            _defaultPlayerSpawnPoint.rotation :
            Quaternion.identity;

        Player = UnityEngine.Object.Instantiate(_sceneConstructionKit.playerPrefab, spawnPosition, spawnRotation);
    }


    private void SpawnCamera()
    {
        Vector3 spawnPosition = _defaultCameraSpawnPoint != null ?
            _defaultCameraSpawnPoint.position :
            Vector3.zero;

        Quaternion spawnRotation = _defaultCameraSpawnPoint != null ?
            _defaultCameraSpawnPoint.rotation :
            Quaternion.identity;

        GameObject camera = UnityEngine.Object.Instantiate(_sceneConstructionKit.cameraPrefab, spawnPosition, spawnRotation);
        camera.GetComponent<CameraController>().Init(Player.transform, _sceneConstructionKit.cameraConfig);
    }

}
public class SceneConstructionKit
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public CharacterPlayerConfig playerConfig;
    public CameraConfig cameraConfig;
}