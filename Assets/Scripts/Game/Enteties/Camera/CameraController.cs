using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraConfig _cameraConfig;
    private Transform _player;
    private Vector3 velocity = Vector3.zero;

    public void Init(Transform player, CameraConfig cameraConfig)
    {
        _player = player;
        _cameraConfig = cameraConfig;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (_player != null)
        {
            float targetY = _player.position.y + _cameraConfig.Offset.y;

            if (targetY > transform.position.y)
            {
                Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, _cameraConfig.SmoothSpeed);
            }
        }
    }
}
