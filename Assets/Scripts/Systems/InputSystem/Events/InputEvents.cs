using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Static events container for all input actions
/// </summary>
public static class InputEvents
{    
    // Movement events
    public static event Action<Vector2> OnMovePlayer;

    // Action events
    public static void MovePlayer(Vector2 direction) => OnMovePlayer?.Invoke(direction); 
}