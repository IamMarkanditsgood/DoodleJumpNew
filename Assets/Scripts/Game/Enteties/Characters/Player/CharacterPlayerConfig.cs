using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/GamePlayManager/Enteties/Character/Player", order = 1)]
public class CharacterPlayerConfig : ScriptableObject
{
    [Tooltip("The force applied when the player jumps. Higher values result in higher jumps.")]
    [SerializeField] private float _jumpForce = 10f;

    [Tooltip("The horizontal movement speed of the player.")]
    [SerializeField] private float _moveSpeed = 5f;

    [Tooltip("The distance below the camera at which the player is considered dead.")]
    [SerializeField] private float _deathDistanceBelowCamera = 5f;

    [Tooltip("Minimum Y value of the collision normal to treat the contact as ground.\n" +
             "Y = 1 means the player hit the top of the platform (valid jump contact).\n" +
             "Y = -1 means the player hit the bottom of the platform (invalid for jumping).")]
    [SerializeField, Range(-1, 1)] private float _contactNormalThreshold = 0.1f;

    [Tooltip("The health of player. It is recommended to keep it on 0")]
    [SerializeField] private float _basicHealth = 0f;

    [SerializeField]
    private LayerMask _boosterLayers;

    public float JumpForce => _jumpForce;
    public float MoveSpeed => _moveSpeed;
    public float DeathDistanceBelowCamera => _deathDistanceBelowCamera;
    public float ContactNormalThreshold => _contactNormalThreshold;
    public float BasicHealth => _basicHealth;
    public LayerMask BoosterLayers => _boosterLayers;
}
