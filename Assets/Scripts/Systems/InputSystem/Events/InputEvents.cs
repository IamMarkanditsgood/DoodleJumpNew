using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Static events container for all input actions
/// </summary>
public static class InputEvents
{    
    // Movement events
    public static event Action<Vector2> OnMouseMovement;
    public static event Action<Vector2> OnKeyBordMovement;

    // Action events

    public static void MouseMovement(Vector2 movementDirection) => OnMouseMovement?.Invoke(movementDirection);
    public static void KeyboardMovement(Vector2 movementDirection) => OnKeyBordMovement?.Invoke(movementDirection);
}