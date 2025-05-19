using UnityEngine;
/// <summary>
/// Handles all movement physics and calculations
/// Doesn't contain input logic, only applies movement forces
/// </summary>
[System.Serializable]
public class CharacterMovementManager
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;

    private float _horizontalInput;

    public Rigidbody2D RigidBody => _rigidbody;

    public void Initialize(Rigidbody2D rb, Collider2D collider)
    {
        _rigidbody = rb;
        _collider = collider;
    }

    /// <summary>
    /// Applies horizontal movement based on input
    /// Should be called in FixedUpdate
    /// </summary>
    public void Move(float horizontalInput, float moveSpeed)
    {
        _horizontalInput = horizontalInput;

        _rigidbody.velocity = new Vector2(
            _horizontalInput * moveSpeed,
            _rigidbody.velocity.y
        );
    }

    public void CheckGroundStatus(float groundCheckOffset)
    {
        var bounds = _collider.bounds;
        var rayStart = new Vector2(bounds.center.x, bounds.min.y);
        var rayLength = groundCheckOffset;

        var hit = Physics2D.Raycast(rayStart, Vector2.down, rayLength);
    }
    /// <summary>
    /// Makes character jump if grounded
    /// </summary>
    public void Jump(float jumpForce)
    {
        _rigidbody.velocity = new Vector2(
            _rigidbody.velocity.x,
            jumpForce
        );
    }
}