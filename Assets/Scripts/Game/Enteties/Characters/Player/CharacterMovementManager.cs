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

    public void HandleCollision(Collision2D collision, float groundCheckOffset, float jumpForce)
    {
        if (!collision.gameObject.CompareTag(GameTags.instantiate.PlatformTag) && !collision.gameObject.CompareTag(GameTags.instantiate.EnemyTag)) return;

        if (IsCollisionBelow(collision, groundCheckOffset))
        {
            Jump(jumpForce);
        }
    }

    private bool IsCollisionBelow(Collision2D collision, float tolerance)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > tolerance)
            {
                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// Makes character jump if grounded
    /// </summary>
    private void Jump(float jumpForce)
    {
        _rigidbody.velocity = new Vector2(
            _rigidbody.velocity.x,
            jumpForce
        );
    }
}