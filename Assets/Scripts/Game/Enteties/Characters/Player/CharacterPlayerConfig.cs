using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/GamePlayManager/Enteties/Character/Player", order = 1)]
public class CharacterPlayerConfig : ScriptableObject
{
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _deathDistanceBelowCamera = 5f;
    [SerializeField] private float _groundCheckOffset = 0.1f;
    public float JumpForce => _jumpForce;
    public float MoveSpeed => _moveSpeed;
    public float DeathDistanceBelowCamera => _deathDistanceBelowCamera;
    public float GroundCheckOffset => _groundCheckOffset;
}
