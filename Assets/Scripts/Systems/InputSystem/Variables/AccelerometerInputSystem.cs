using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerInputSystem : IInputable
{
    private const float ACCELEROMETER_SENSITIVITY = 5f;

    public void CheckInput()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        Vector2 acceleration = Input.acceleration;

        float xMovement = acceleration.x * ACCELEROMETER_SENSITIVITY;

        if (Mathf.Abs(xMovement) > 0.05f)
        {
            Vector2 moveDelta = new Vector2(xMovement, 0f);
            InputEvents.MovePlayer(moveDelta);
        }
    }
}
