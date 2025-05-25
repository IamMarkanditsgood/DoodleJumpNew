using UnityEngine;

/// <summary>
/// Handles mouse movement and button presses
/// </summary>
public class MouseInputSystem : IInputable
{
    private const float STANDART_MOUSE_SENSITIVITY = 1f;

    public void CheckInput()
    {
        CheckMovement();
        CheckButtons();
    }

    private void CheckMovement()
    {
        Vector2 mouseDelta = new Vector2(
            Input.GetAxis("Mouse X") * STANDART_MOUSE_SENSITIVITY,
            Input.GetAxis("Mouse Y") * STANDART_MOUSE_SENSITIVITY
        );

        InputEvents.MovePlayer(mouseDelta);
    }

    private void CheckButtons()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }

        if (Input.GetMouseButtonDown(1))
        {
        }
    }
}