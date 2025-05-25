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
    [SerializeField] private List<GameStage> _gameStages;

    [SerializeField] private float _gameTime = 0f;

    private GameStage _currentStage;

    private bool _canSpawnEnemy = false;

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
            List<PlatformTypes> platformsInRom = new List<PlatformTypes> { PlatformTypes.defaultPlatform, PlatformTypes.defaultPlatform, PlatformTypes.defaultPlatform, PlatformTypes.defaultPlatform, PlatformTypes.defaultPlatform };
            SpawnPatform(platformsInRom);
        }
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            _gameTime += 1;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SpawnSceneObjects()
    {
        while (true)
        {
            _currentStage = GetCurrentStage();

            if (_currentStage != null)
            {
                if (_currentStage.platformsInStage.Count > 0)
                {
                    List<PlatformTypes> platformsInRow = new List<PlatformTypes>();

                    int countOfPlatforms = (int) UnityEngine.Random.Range(_currentStage.platformsInRow.x, _currentStage.platformsInRow.y);

                    for (int i = 0; i < countOfPlatforms; i++)
                    {
                        PlatformData selectedPlatform = GetRandomByDifficulty(_currentStage.platformsInStage, pd => pd.difficulty);
                        platformsInRow.Add(selectedPlatform.type);
                    }

                    bool addBooseter = UnityEngine.Random.value < _currentStage.boostersSpawnChance;
                    SpawnPatform(platformsInRow, addBooseter);
                }

                if (_currentStage.enemiesInStage.Count > 0)
                {
                    EnemyData selectedEnemy = GetRandomByDifficulty(_currentStage.enemiesInStage, ed => ed.difficulty);
                    SpawnEnemy(selectedEnemy.type);
                }               
            }
            yield return new WaitForSeconds(_gameConfig.SpawnIntervalTimer);
        }
    }
    private GameStage GetCurrentStage()
    {
        GameStage result = null;
        foreach (var stage in _gameStages)
        {
            if (_gameTime >= stage.startTime)
            {
                result = stage;
            }
            else
            {
                break;
            }
        }
        return result;
    }

    private void SpawnPatform(List<PlatformTypes> platformsInRom, bool addBuster = false)
    { 
        if(_platformsSpawnManager.IsSpawnAvailable())
        {
            _canSpawnEnemy = true;
            _platformsSpawnManager.SpawnPlatforms(platformsInRom, addBuster);
        }
    }

    private void SpawnEnemy(EnemyTypes enemyType)
    {
        if (_canSpawnEnemy)
        {
            _canSpawnEnemy = false;

            if (UnityEngine.Random.value < _currentStage.enemySpawnChance)
            {
                GameObject platform = _platformsSpawnManager.GetPlatformInRow(_platformsSpawnManager.PrewSpawnY);
                float xPos = platform.transform.position.x;

                _enemySpawnManager.SpawnEnemy(enemyType, _platformsSpawnManager.PrewSpawnY + 1f, xPos);
            }
        }
    }

    private T GetRandomByDifficulty<T>(List<T> items, Func<T, int> difficultySelector)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("GetRandomByDifficulty was called with an empty or null list.");
            return default;
        }

        float totalWeight = 0f;
        List<float> weights = new List<float>();

        foreach (var item in items)
        {
            int difficulty = difficultySelector(item);
            float weight = 1f / Mathf.Max(difficulty, 1); // уникнути д≥ленн€ на 0
            weights.Add(weight);
            totalWeight += weight;
        }

        float rand = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < items.Count; i++)
        {
            cumulative += weights[i];
            if (rand <= cumulative)
                return items[i];
        }

        // fallback Ц тепер без виклику Last()
        return items[UnityEngine.Random.Range(0, items.Count)];
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
[Serializable]
public class GameStage
{
    public string stageName;
    public int startTime; // in Seconds
    public List<PlatformData> platformsInStage;
    public List<EnemyData> enemiesInStage;
    [Range (0, 1)]public float enemySpawnChance;
    [Range(0, 1)] public float boostersSpawnChance;
    public Vector2 platformsInRow;
}