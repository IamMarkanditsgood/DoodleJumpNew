using System;
using UnityEngine;
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
