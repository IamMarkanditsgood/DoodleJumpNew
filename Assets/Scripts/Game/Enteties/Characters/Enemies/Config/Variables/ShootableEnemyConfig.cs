using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootableEnemy", menuName = "ScriptableObjects/GamePlayManager/Enteties/Enemies/ShootableEnemy", order = 1)]
public class ShootableEnemyConfig : BasicEnemyConfig
{
    [SerializeField] private float _shootingDelay;
    [SerializeField] private BasicAmmoConfig[] _ammoConfigs;
    [SerializeField] private AmmoTypes _currentAmmo;

    public float ShootingDelay => _shootingDelay;
    public BasicAmmoConfig[] AmmoConfigs => _ammoConfigs;
    public AmmoTypes CurrentAmmo => _currentAmmo;
}