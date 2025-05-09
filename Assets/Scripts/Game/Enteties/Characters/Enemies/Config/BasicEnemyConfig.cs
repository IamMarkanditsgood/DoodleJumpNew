using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyTypes _enemyType;
    [SerializeField] private bool _canDie;

    public EnemyTypes EnemyType => _enemyType;
    public bool CanDie => _canDie;
}