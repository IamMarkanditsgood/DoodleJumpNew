using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [Tooltip("How smooth the camera follows the player. Higher values result in faster following.")]
    public float smoothSpeed = 0.1f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player != null)
        {
            float targetY = player.position.y + offset.y;

            // ѕровер€ем: игрок выше текущей позиции камеры?
            if (targetY > transform.position.y)
            {
                Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            }
        }
    }
}
