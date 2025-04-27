using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main input manager that aggregates all input systems
/// Implements modular input handling through IInputable interfaces
/// </summary>
[Serializable]
public class PlayerInputSystem
{
    [Header("Debug")]
    [SerializeField] private bool _logInputEvents;

    private List<IInputable> _inputSystems = new List<IInputable>();

    /// <summary>
    /// Initializes all input systems based on platform
    /// </summary>
    public void Init()
    {
        DeclareInputSystems();

        if (_logInputEvents)
            Debug.Log($"[Input] Systems initialized: {_inputSystems.Count}");
    }

    /// <summary>
    /// Checks all inputs every frame (should be called from Update)
    /// </summary>
    public void UpdateInput()
    {
        foreach (var system in _inputSystems)
        {
            system.CheckInput();
        }
    }

    /// <summary>
    /// Registers input systems based on current platform
    /// </summary>
    private void DeclareInputSystems()
    {
        _inputSystems.Clear();

#if UNITY_EDITOR || UNITY_STANDALONE
        _inputSystems.Add(new KeyboardInputSystem());
        _inputSystems.Add(new MouseInputSystem());
#elif UNITY_ANDROID || UNITY_IOS
        _inputSystems.Add(new TouchInputSystem()); // Пример для будущей реализации
#endif
    }
}