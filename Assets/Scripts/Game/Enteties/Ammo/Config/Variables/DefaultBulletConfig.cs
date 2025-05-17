using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBulletConfig", menuName = "ScriptableObjects/GamePlayManager/Enteties/Ammo/DefaultBulletConfig", order = 1)]
public class DefaultBulletConfig : BasicAmmoConfig
{
    [SerializeField] private Vector2 _movementDirection;
    [Tooltip("Speed of bullet movement. It should be positive!")]
    [SerializeField] private float _speed;

    public Vector2 MovementDirection => _movementDirection;
    public float Speed => _speed;
}