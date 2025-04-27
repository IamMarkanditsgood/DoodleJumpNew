using UnityEngine;

[CreateAssetMenu(fileName = "Camera", menuName = "ScriptableObjects/GamePlayManager/Enteties/Camera", order = 1)]
public class CameraConfig : ScriptableObject
{
    [Tooltip("How smooth the camera follows the _player. Higher values result in faster following.")]
    [SerializeField] private float _smoothSpeed = 0.1f;
    [Tooltip("Vertical (Y) and horizontal (X) offset from player position.\n" +"Y - how high above the player the camera stays\n" + "X - horizontal shift from player center (0 for perfect center)")]
    [SerializeField] private Vector2 _offset;

    public float SmoothSpeed => _smoothSpeed;
    public Vector2 Offset => _offset;
}