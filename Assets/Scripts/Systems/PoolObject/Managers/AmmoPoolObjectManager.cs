using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AmmoPoolObjectManager 
{
    [Header("Containers")]
    [SerializeField] private Transform _ammoContainer;

    private List<AmmoData> _ammoPoolObjects = new();

    public class AmmoData
    {
        public ObjectPool<BasicAmmoController> ammunitionsPool = new ObjectPool<BasicAmmoController>();
        public AmmoTypes Type;
    }

    public void InitPools(BasicAmmoController[] ammunitions)
    {
        foreach (var ammo in ammunitions)
        {
            AmmoData newAmmoData = new AmmoData();
            newAmmoData.Type = ammo.AmmoType;
            newAmmoData.ammunitionsPool.InitializePool(ammo, _ammoContainer, 30);

            _ammoPoolObjects.Add(newAmmoData);
        }
    }

    public BasicAmmoController GetAmmo(AmmoTypes type)
    {
        foreach (var pool in _ammoPoolObjects)
        {
            if (pool.Type == type)
            {
                return pool.ammunitionsPool.GetFreeComponent();
            }
        }

        Debug.LogError($"There is no {type} type of ammo in pool!");
        return null;
    }

    public void DisableAmmo(BasicAmmoController ammo, AmmoTypes type)
    {
        foreach (var pool in _ammoPoolObjects)
        {
            if (pool.Type == type)
            {
                pool.ammunitionsPool.DisableComponent(ammo);
                return;
            }
        }

        Debug.LogError($"There is no {type} type of ammo in pool!");
    }
}
