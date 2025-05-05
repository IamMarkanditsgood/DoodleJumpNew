using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterPlayerController : MonoBehaviour, IHitable
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CharacterMovementManager _movementEngine;
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

        _movementEngine.CheckGroundStatus(_playerConfig.ContactNormalThreshold);
    }

    private void FixedUpdate()
    {
        // Physics-related movement in FixedUpdate
        _movementEngine.Move(_horizontalInput, _playerConfig.MoveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _movementEngine.HandleCollision(collision,
            _playerConfig.ContactNormalThreshold,
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

    public void Hit(float damage = 0)
    {
        Die();
    }
}