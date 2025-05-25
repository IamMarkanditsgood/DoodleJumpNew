using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputSystem : IInputable
{
    private const float TOUCH_SENSITIVITY = 1f;

    public void CheckInput()
    {
        CheckMovement();

    }

    private void CheckMovement()
    {
        Vector2 delta;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                delta = touch.deltaPosition * TOUCH_SENSITIVITY;
                InputEvents.MovePlayer(delta);
                return;
            }
        }

        delta = Vector2.zero;
        InputEvents.MovePlayer(delta);
    }

}
