using UnityEngine;
/// <summary>
/// Handles all keyboard input including movement and actions
/// </summary>
public class KeyboardInputSystem : IInputable
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    public void CheckInput()
    {
        CheckMovement();
        CheckButtonsInput();
    }

    private void CheckMovement()
    {
        Vector2 movementDirection = new Vector2(
            Input.GetAxis(HORIZONTAL_AXIS),
            Input.GetAxis(VERTICAL_AXIS)
        );

        InputEvents.MovePlayer(movementDirection.normalized);
    }

    private void CheckButtonsInput()
    {
    }
}