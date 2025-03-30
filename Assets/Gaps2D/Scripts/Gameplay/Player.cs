using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public Rigidbody2D playerRigidbody2D;
    public SpriteRenderer playerSprite;
    public Material trailMaterial;
    [Space(5)]
    public GameObject sleepParticles;
    public GameObject crashParticles;
    [Space(5)]
    public GameObject wingsEffect;
    public Animator animator;
    [Space(5)]
    public Vector2 targetPosition;

    public bool revive, flyToPosition;

    GameObject obstacleHit;
    Vector2 dir;

    //player go out of screen (bottom)
    void OnBecameInvisible()
    {
        if (GameManager.Instance.uIManager.gameState == GameState.PLAYING)
            GameManager.Instance.uIManager.ShowRevive();
    }

    void Update()
    {
        //set player rotation to 0
        transform.eulerAngles = new Vector3(0,0,0);
    }

    public void PlayerPause(bool enable)
    {
        sleepParticles.SetActive(enable);
    }

    //reset player
    public void Reset()
    {
        trailRenderer.enabled = true;
        playerSprite.enabled = true;
        DisableFlying();
        sleepParticles.SetActive(false);
    }

    //shoot the player
    public void Shoot()
    {
         transform.parent = null;
         dir = transform.position - obstacleHit.transform.position;
         dir.Normalize();

         playerRigidbody2D.AddForce(dir * GameManager.Instance.playerJumpSpeed, ForceMode2D.Impulse);    
    }

    //set player skin
    public void SetSkin(PlayerSkin skin)
    {
        playerSprite.sprite = skin.playerSprite;
        trailMaterial.color = skin.trailColor;
    }

    //collision with solid colliders
    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Walls") && !revive)
       {
            //create crash particles
            GameObject tempParticles = Instantiate(crashParticles);
            tempParticles.transform.position = gameObject.transform.position;
            AudioManager.Instance.PlayEffects(AudioManager.Instance.crash);
            GameManager.Instance.uIManager.ShowRevive();
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerRigidbody2D.velocity = Vector2.zero;
            transform.parent = collision.gameObject.transform;
            obstacleHit = collision.gameObject;

            if (flyToPosition)
            {
                revive = false;

                GameManager.Instance.uIManager.gameState = GameState.PLAYING;
                DisableFlying();
            }

            AudioManager.Instance.PlayEffects(AudioManager.Instance.hitCircle);
            GameManager.Instance.HitObstacle();
        }
    }

    //collision with triggers
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            CoinManager.Instance.PickCoin(1);
            Destroy(collision.gameObject);
        }
    }

    //fly towards the obstacle/target position
    IEnumerator FlyTowardsObstacle(int speed)
    {
        playerRigidbody2D.velocity = Vector2.zero;
        flyToPosition = true;

        while (Vector2.Distance(targetPosition, transform.position) > .1f)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

            if (!flyToPosition)
                yield break;

            yield return new WaitForEndOfFrame();
        }

        revive = false;

        GameManager.Instance.uIManager.gameState = GameState.PLAYING;

        DisableFlying();
    }

    //disable flying effetcs 
    public void DisableFlying()
    {
        flyToPosition = false;
        wingsEffect.SetActive(false);
    }

    //revive player
    public void Revive()
    {
        AudioManager.Instance.PlayEffects(AudioManager.Instance.revive);
        playerSprite.enabled = true;
        ActivateWings();
        revive = true;

        targetPosition = (Vector2)obstacleHit.transform.position;
        StartCoroutine(FlyTowardsObstacle(1));
    }

    //activate wings
    public void ActivateWings()
    {
        wingsEffect.SetActive(true);
        animator.Play("Wings");
    }
}