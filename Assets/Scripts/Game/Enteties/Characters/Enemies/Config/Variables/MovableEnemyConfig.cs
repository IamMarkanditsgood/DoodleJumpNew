using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovableEnemy", menuName = "ScriptableObjects/GamePlayManager/Enteties/Enemies/MovableEnemy", order = 1)]
public class MovableEnemyConfig : BasicEnemyConfig
{
    [Tooltip("The horizontal movement speed of the platform (units per second).It should be positive!")]
    [SerializeField] private float _basicSpeed = 2f;
    [Tooltip("Enable to use custom movement limits. If disabled, camera bounds will be used.")]
    [SerializeField] private bool _useCustomMovementLimits = false;
    [Tooltip("The left boundary for platform movement (used only if custom limits are enabled).")]
    [SerializeField] private float _leftMovementLimit = -5f;
    [Tooltip("The right boundary for platform movement (used only if custom limits are enabled).")]
    [SerializeField] private float _righMovementtLimit = 5f;
    [Tooltip("Initial movement direction of the platform: 1 for right, -1 for left.")]
    [SerializeField] private float _startDirection = 1f;

    public float BasicSpeed => _basicSpeed;
    public bool UseCustomMovementLimits => _useCustomMovementLimits;
    public float LeftLimit => _leftMovementLimit;
    public float RightLimit => _righMovementtLimit;
    public float StartDirection
    {
        get
        {
            if (_startDirection != 1f && _startDirection != -1f)
            {
                Debug.LogWarning("StartDirection can only be 1 or -1. Automatically adjusted.");
                return Mathf.Sign(_startDirection) != 0 ? Mathf.Sign(_startDirection) : 1f;
            }
            return _startDirection;
        }
    }
}
