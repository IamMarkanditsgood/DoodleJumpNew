using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterPlayerController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CharacterMovementEngine _movementEngine;
    [SerializeField] private PlayerInputSystem _playerInputSystem;

    private CharacterPlayerConfig _playerConfig;
   
    private float _screenWidthInUnits;
    private float _horizontalInput;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void Init(CharacterPlayerConfig characterPlayerConfig)
    {
        _playerConfig = characterPlayerConfig;

        if (!_mainCamera) _mainCamera = Camera.main;
        _movementEngine.Initialize(GetComponent<Rigidbody2D>(), GetComponent<Collider2D>());

        _playerInputSystem.Init();
    }

    public void Subscribe()
    {
        InputEvents.OnKeyBordMovement += OnMoveInput;
    }
    public void Unsubscribe()
    {
        InputEvents.OnKeyBordMovement -= OnMoveInput;
    }

    private void Update()
    {
        _playerInputSystem.UpdateInput();

        CalculateScreenBounds();
        HandleScreenWrapping();
        CheckFallDeath();

        _movementEngine.CheckGroundStatus(_playerConfig.GroundCheckOffset);
    }

    private void FixedUpdate()
    {
        // Physics-related movement in FixedUpdate
        _movementEngine.Move(_horizontalInput, _playerConfig.MoveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _movementEngine.HandleCollision(collision,
            _playerConfig.GroundCheckOffset,
            _playerConfig.JumpForce);
    }

    private void OnMoveInput(Vector2 direction)
    {
        _horizontalInput = direction.x;
    }

    private void CalculateScreenBounds()
    {
        float screenHeight = _mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        _screenWidthInUnits = screenWidth / 2f;
    }

    private void HandleScreenWrapping()
    {
        if (transform.position.x > _screenWidthInUnits)
            transform.position = new Vector2(-_screenWidthInUnits, transform.position.y);
        else if (transform.position.x < -_screenWidthInUnits)
            transform.position = new Vector2(_screenWidthInUnits, transform.position.y);
    }

    private void CheckFallDeath()
    {
        float cameraBottom = _mainCamera.transform.position.y - _mainCamera.orthographicSize;
        if (transform.position.y < cameraBottom - _playerConfig.DeathDistanceBelowCamera)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Died!");
        Time.timeScale = 0;
    }
}
/// <summary>
/// Handles all movement physics and calculations
/// Doesn't contain input logic, only applies movement forces
/// </summary>
[System.Serializable]
public class CharacterMovementEngine
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;

    private float _horizontalInput;
    private bool _isGrounded;

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
        _isGrounded = hit.collider != null && hit.collider.CompareTag("Platform");
    }

    public void HandleCollision(Collision2D collision, float groundCheckOffset, float jumpForce)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;

        if (IsCollisionBelow(collision, groundCheckOffset))
        {
            Jump(jumpForce);
        }
    }

    private bool IsCollisionBelow(Collision2D collision, float tolerance)
    {
        float playerBottom = _collider.bounds.min.y;
        float platformTop = collision.collider.bounds.max.y;
        return playerBottom <= platformTop + tolerance;
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
        _isGrounded = false;
    }
}