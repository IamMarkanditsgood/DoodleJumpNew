using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GamePlayManager
{
    [SerializeField] private PlatformsSpawnManager _platformsSpawnManager;
    [SerializeField] private EnemySpawnManager _enemySpawnManager;

    [SerializeField] private List<PlatformData> _allPlatforms;
    [SerializeField] private List<EnemyData> _allEnemies;

    [SerializeField] private float _difficultyIncreaseRate = 1f; // щосекунди або кожні N очок

    [SerializeField] private float _currentDifficulty = 0;
    private float _gameTime = 0f;

    private Camera _mainCamera;
    private GameConfig _gameConfig;
   
    private GameObject _player;

    private Coroutine _spawnPlatforms;
    private Coroutine _objectGarbageCollector;
    private Coroutine _gameTimer;

    public bool IsGameStarted { get; private set; }

    public void Init(GameConfig gameConfig, GameObject player)
    {
        _player = player;

        _gameConfig = gameConfig;

        _mainCamera = Camera.main;

        _platformsSpawnManager.Init(player.transform, _mainCamera, _gameConfig);
        _enemySpawnManager.Init(player.transform, _mainCamera, _gameConfig);
    }

    public void DeInit()
    {
        CoroutineServices.instance.StopRoutine(_spawnPlatforms);
        CoroutineServices.instance.StopRoutine(_objectGarbageCollector);
        CoroutineServices.instance.StopCoroutine(_gameTimer);
    }

    public void StartGame()
    {
        IsGameStarted = true;
        
        FirstSpawn();

         _spawnPlatforms = CoroutineServices.instance.StartRoutine(SpawnSceneObjects());
        _objectGarbageCollector = CoroutineServices.instance.StartRoutine(ObjectGarbageCollector());
        _gameTimer = CoroutineServices.instance.StartRoutine(GameTimer());
    }

    private void FirstSpawn()
    {
        for (int i = 0; i < 5; i++)
        {
            List<PlatformTypes> platformsInRom = new List<PlatformTypes> { PlatformTypes.broken, PlatformTypes.movable, PlatformTypes.defaultPlatform };
            _platformsSpawnManager.SpawnPlatforms(platformsInRom, true);

            /*GameObject platform = _platformsSpawnManager.GetPlatformInRow(_platformsSpawnManager.PrewSpawnY);
            float xPos = platform.transform.position.x;
            _enemySpawnManager.SpawnEnemy(EnemyTypes.shootableEnemy, _platformsSpawnManager.PrewSpawnY + 1f, xPos);*/
        }
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            _gameTime += Time.deltaTime;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SpawnSceneObjects()
    {
        while (true)
        {
            /*List<PlatformTypes> platformsInRom = new List<PlatformTypes> { PlatformTypes.broken };
            _platformsSpawnManager.SpawnPlatforms(platformsInRom);*/


            // Підвищуємо складність поступово
            _currentDifficulty = Mathf.Clamp(_gameTime * _difficultyIncreaseRate, 0, 100);

            SpawnGameplayObjects();
            yield return new WaitForSeconds(_gameConfig.SpawnIntervalTimer);
        }
    }
    private IEnumerator ObjectGarbageCollector()
    {
        while (true)
        {
            _platformsSpawnManager.ClearPlatformsGarbage(_player.transform);
            _enemySpawnManager.ClearEnemiesGarbage(_player.transform);

            yield return new WaitForSeconds(_gameConfig.ObjectGarbageCollectorInterval);
        }
    }

    private void SpawnGameplayObjects()
    {
        // Вибираємо платформи відповідної складності
        List<PlatformData> availablePlatforms = _allPlatforms
            .Where(p => p.difficulty <= _currentDifficulty)
            .ToList();

        int platformCount = Mathf.RoundToInt(Mathf.Lerp(3, 1, _currentDifficulty / 100f));
        platformCount = Mathf.Clamp(platformCount, 1, 3);

        List<PlatformTypes> selectedPlatforms = GetRandomPlatforms(availablePlatforms, platformCount);


        // 20% шанс додати бустер (залежить від складності)
        bool addBooster = UnityEngine.Random.value < 0.01f;

        _platformsSpawnManager.SpawnPlatforms(selectedPlatforms, addBooster);

        // Шанс спавнити ворога зростає зі складністю
        float enemySpawnChance = Mathf.Lerp(0.01f, 0.7f, _currentDifficulty / 100f);
        if (UnityEngine.Random.value < enemySpawnChance)
        {
            List<EnemyData> availableEnemies = _allEnemies
                .Where(e => e.difficulty <= _currentDifficulty)
                .ToList();

            if (availableEnemies.Count > 0)
            {
                EnemyData selectedEnemy = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];

                GameObject platform = _platformsSpawnManager.GetPlatformInRow(_platformsSpawnManager.PrewSpawnY);
                float xPos = platform.transform.position.x;

                _enemySpawnManager.SpawnEnemy(selectedEnemy.type, _platformsSpawnManager.PrewSpawnY + 1f, xPos);
            }
        }
    }

    private List<PlatformTypes> GetRandomPlatforms(List<PlatformData> source, int count)
    {
        List<PlatformTypes> result = new List<PlatformTypes>();
        for (int i = 0; i < count; i++)
        {
            result.Add(source[UnityEngine.Random.Range(0, source.Count)].type);
        }
        return result;
    }
}
[System.Serializable]
public class EnemyData
{
    public EnemyTypes type;
    [Range(0, 100)] public int difficulty;
}
[System.Serializable]
public class PlatformData
{
    public PlatformTypes type;
    [Range(0, 100)] public int difficulty;
}