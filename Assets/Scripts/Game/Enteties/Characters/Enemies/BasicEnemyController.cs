using UnityEngine;

public abstract class BasicEnemyController : MonoBehaviour, ICustomisable
{
    [SerializeField] private EnemyTypes _enemyType;

    private BasicEnemyConfig _basicEnemyConfig;
    private bool _isActive;

    public EnemyTypes EnemyType => _enemyType;

    //"Minimum Y value of the collision normal to treat the contact." 
    //"Y = -1 means the player hit the top of the enemy." 
    //"Y = 1 means the player hit the bottom of the enemy."            
    private const float _contactNormalThreshold = 0.1f;

    public virtual void Init(BasicEnemyConfig basicEnemyConfig)
    {
        _basicEnemyConfig = basicEnemyConfig;
        SetCustomisation();
    }

    public virtual void Toggle(bool state)
    {
        _isActive = state;

        if(!_isActive)
        {
            StopAllCoroutines();
        }    
    }

    private void Update()
    {
        if (!_isActive)
            return;

        UpdateEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isActive)
            return;

        HandleContact(collision);
    }

    public virtual void UpdateEnemy() { }

    public virtual void HandleContact(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(GameTags.instantiate.PlayerTag)) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 contactNormal = contact.normal;

            // hit from top
            if (contactNormal.y < -_contactNormalThreshold)
            {
                if(_basicEnemyConfig.CanDie)
                    Die();  

                break;
            }
            // hit from botton
            else if(contactNormal.y > _contactNormalThreshold)
            {
                HitPlayer(collision);
                break;
            }
        }
    }

    public virtual void HitPlayer(Collision2D collision)
    {
        IHitable hitablePlayer = collision.gameObject.GetComponent<IHitable>();

        if (hitablePlayer != null)
            hitablePlayer.Hit();
    }

    public virtual void Die()
    {
        Toggle(false);
        PoolObjectManager.instant.enemyPoolObjectManager.DisableEnemy(this, _enemyType);
    }

    public void SetCustomisation()
    {
        ICustomizer customizer = GetComponent<ICustomizer>();

        if (customizer != null)
            customizer.Customize();
    }
}