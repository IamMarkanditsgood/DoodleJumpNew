using System;
using System.Collections;
using UnityEngine;

public class ShootableEnemy : BasicEnemyController
{
    [SerializeField] private Transform _shootingPos;

    private ShootableEnemyConfig _shootableEnemyConfig;

    private Coroutine _shootCoroutine;

    public override void Init(BasicEnemyConfig basicEnemyConfig)
    {
        base.Init(basicEnemyConfig);

        _shootableEnemyConfig = basicEnemyConfig as ShootableEnemyConfig;

        if (_shootableEnemyConfig == null)
        {
            throw new InvalidCastException($"[Init] _shootableEnemyConfig is not of type ShootableEnemyConfig in {gameObject.name}");
        }
    }

    public override void Toggle(bool state)
    {
        base.Toggle(state);

        if(state)
        {
            _shootCoroutine = StartCoroutine(Shoot());
        }
        else if (!state && _shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
        }
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            BasicAmmoController newBullet = PoolObjectManager.instant.ammoPoolObjectManager.GetAmmo(AmmoTypes.bullet);

            newBullet.gameObject.transform.SetPositionAndRotation(_shootingPos.position, _shootingPos.rotation);
            newBullet.Init(GetAmmoConfigByType(_shootableEnemyConfig.CurrentAmmo));
            newBullet.Toggle(true);

            yield return new WaitForSeconds(_shootableEnemyConfig.ShootingDelay);
        }
    }

    private BasicAmmoConfig GetAmmoConfigByType(AmmoTypes type)
    {
        foreach(var config in _shootableEnemyConfig.AmmoConfigs)
        {
            if(type == config.AmmoType)
            {
                return config;
            }
        }

        Debug.LogError($"{gameObject.name} does not contain ammo config with type {type} in {this} script!");
        return null;
    }

}