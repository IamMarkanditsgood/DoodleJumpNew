using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterPlayerController : MonoBehaviour, IHitable
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CharacterMovementManager _movementEngine;
    [SerializeField] private PlayerInputSystem _playerInputSystem;
    
    private CharacterPlayerConfig _playerConfig;

    private BoosterUseManager _boosterUseManager = new BoosterUseManager();
    private Collider2D _playerCollider;

    private float _currentHealth;
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

        _playerCollider = gameObject.GetComponent<Collider2D>();
        Rigidbody2D playerRB = GetComponent<Rigidbody2D>();

        if (!_mainCamera) _mainCamera = Camera.main;

        _movementEngine.Initialize(playerRB, _playerCollider);
        _boosterUseManager.Init(playerRB);

        _playerInputSystem.Init();

        _currentHealth = _playerConfig.BasicHealth;
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
        HandleCollision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrigger(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if(!IsInteractableCollision(collision)) return;
        
        if (IsCollisionBelow(collision, _playerConfig.ContactNormalThreshold))
        {

            _movementEngine.Jump(_playerConfig.JumpForce);
        }
    }

    private void HandleTrigger(Collider2D collider)
    {
        if (!IsInLayerMask(collider.gameObject, _playerConfig.BoosterLayers)) return;

        BasicBoosterController booster = collider.gameObject.GetComponent<BasicBoosterController>();
        if (booster != null)
            TryToUseBooster(booster);
    }

    private bool IsInteractableCollision(Collision2D collision)
    {
        return collision.gameObject.CompareTag(GameTags.instantiate.PlatformTag) ||
           collision.gameObject.CompareTag(GameTags.instantiate.EnemyTag);
    }
    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
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
    private bool IsColliderBelow(Collider2D collider, float tolerance, Transform self)
    {
        Vector2 directionToCollider = (collider.bounds.center - self.position).normalized;
        return directionToCollider.y < tolerance;
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
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void TryToUseBooster(BasicBoosterController booster)
    {
        booster.Interact();

        switch(booster.BoosterConfig.BoosterType)
        {
            case BoosterTypes.Spring:
                _boosterUseManager.UseSpring(booster.BoosterConfig);
                break;
            case BoosterTypes.Helicopter:
                _boosterUseManager.UseHelicopter(booster.BoosterConfig);
                break;
            case BoosterTypes.Jetpack:
                _boosterUseManager.UseJetpack(booster.BoosterConfig);
                break;
        }
    }
}
public class BoosterUseManager
{
    private Rigidbody2D _playerRb;
    private Coroutine _boosterCoroutine;
    public void Init(Rigidbody2D playerRb)
    {
        _playerRb = playerRb;
    }

    public void UseSpring(BasicBoosterConfig boosterConfig)
    {
        if (_playerRb == null) return;

        _playerRb.velocity = new Vector2(_playerRb.velocity.x, 0f);
        _playerRb.AddForce(Vector2.up * boosterConfig.BoostJumpForce, ForceMode2D.Impulse);

    }
    public void UseHelicopter(BasicBoosterConfig boosterConfig)
    {
        if (_playerRb == null) return;

        if (_boosterCoroutine != null)
            CoroutineServices.instance.StopRoutine(_boosterCoroutine);

        TimedBoosterConfig timedBoosterConfig = (TimedBoosterConfig)boosterConfig;
        _boosterCoroutine = CoroutineServices.instance.StartRoutine(ApplyVerticalLift(boosterConfig.BoostJumpForce, timedBoosterConfig.TimeOfBoostUse));
    }
    public void UseJetpack(BasicBoosterConfig boosterConfig)
    {
        if (_playerRb == null) return;

        if (_boosterCoroutine != null)
            CoroutineServices.instance.StopRoutine(_boosterCoroutine);

        TimedBoosterConfig timedBoosterConfig = (TimedBoosterConfig)boosterConfig;
        _boosterCoroutine = CoroutineServices.instance.StartRoutine(ApplyVerticalLift(boosterConfig.BoostJumpForce, timedBoosterConfig.TimeOfBoostUse));
    }

    private IEnumerator ApplyVerticalLift(float force, float duration)
    {
        float timer = 0f;
        
        while (timer < duration)
        {
            if (_playerRb == null) yield break;

            _playerRb.velocity = new Vector2(_playerRb.velocity.x, force);

            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        _boosterCoroutine = null;
    }

}