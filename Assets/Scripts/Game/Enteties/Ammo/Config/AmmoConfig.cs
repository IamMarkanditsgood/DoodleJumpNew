using UnityEngine;

public class AmmoConfig : ScriptableObject
{
    [SerializeField] private AmmoTypes _ammoType;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _movementDirection;

    public AmmoTypes AmmoType => _ammoType;
    public float Speed => _speed;
    public Vector2 MovementDirection => _movementDirection;
}