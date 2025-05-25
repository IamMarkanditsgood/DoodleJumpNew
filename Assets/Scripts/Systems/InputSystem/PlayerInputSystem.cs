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
        int CurrentInputSystem = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.InputSystem);

        if(CurrentInputSystem < _inputSystems.Count)
            _inputSystems[CurrentInputSystem].CheckInput();
    }

    /// <summary>
    /// Registers input systems based on current platform
    /// </summary>
    private void DeclareInputSystems()
    {
        _inputSystems.Clear();

#if UNITY_EDITOR   
        _inputSystems.Add(new KeyboardInputSystem());
        _inputSystems.Add(new MouseInputSystem());
#endif
        _inputSystems.Add(new TouchInputSystem());
        _inputSystems.Add(new AccelerometerInputSystem());
    }
}