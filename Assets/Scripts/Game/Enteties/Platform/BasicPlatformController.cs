using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class BasicPlatformController : MonoBehaviour
{
    [SerializeField] private PlatformTypes _platformType;
    [SerializeField] private BasicBoosterConfig[] _boosters;
    [SerializeField] private BasicBoosterController _booster;

    protected BasicPlatformConfig _basicPlatformConfig;

    private bool _isActive;

    public PlatformTypes PlatformType => _platformType;

    public virtual void Init(BasicPlatformConfig basicPlatformConfig)
    {
        _basicPlatformConfig = basicPlatformConfig;
    }

    public virtual void Toggle(bool state)
    {
        _isActive = state;

        if(!_isActive)
        {
            StopAllCoroutines();

            _booster.Toggle(false);
        }
    }

    private void Update()
    {
        if (!_isActive)
            return;

        UpdatePlatform();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isActive)
            return;

        HandleCollision(collision);
    }

    public virtual void UpdatePlatform() { }

    public virtual void HandleCollision(Collision2D collision) { }

    public virtual void AddRandomBuster() 
    {
        if(_booster == null || _boosters.Length == 0)
            return;

        BasicBoosterConfig basicBoosterConfig;
        float totalChance = _boosters.Sum(b => b.UsingChance);
        float randomPoint = Random.Range(0f, totalChance); // √енеруЇмо точку в д≥апазон≥ [0, totalChance)

        float cumulative = 0f;

        foreach (var booster in _boosters)
        {
            cumulative += booster.UsingChance;
            if (randomPoint <= cumulative)
            {
                basicBoosterConfig = booster;

                _booster.Init(basicBoosterConfig);
                _booster.Toggle(true);
                return;
            }
        }

        Debug.LogWarning("No booster was selected. Check chances.");
        return;
    }
}
