using System;
using System.Collections;
using UnityEngine;

public class BrokenPlatform : BasicPlatformController
{
    private BrokenPlatformConfig _brokenPlatformConfig;
    private bool _isBroken = false;

    //"Minimum Y value of the collision normal to treat the contact." 
    //"Y = -1 means the player hit the top of the platform." 
    //"Y = 1 means the player hit the bottom of the platform."            
    private const float _contactNormalThreshold = -0.1f;

    public override void Init(BasicPlatformConfig basicPlatformConfig)
    {
        base.Init(basicPlatformConfig);

        _brokenPlatformConfig = basicPlatformConfig as BrokenPlatformConfig;

        if (_brokenPlatformConfig == null)
        {
            throw new InvalidCastException($"[Init] _brokenPlatformConfig is not of type BrokenPlatformConfig in {gameObject.name}");
        }
    }

    private void DeInit()
    {
        _isBroken = false;
    }

    public override void HandleCollision(Collision2D collision)
    {
        if (_isBroken) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 contactNormal = contact.normal;

            if (contactNormal.y < _contactNormalThreshold)
            {
                BreakPlatform();
                break;
            }
        }
    }

    private void BreakPlatform()
    {
        _isBroken = true;
        StartCoroutine(DestroyPlatformDelayed());
    }

    private IEnumerator DestroyPlatformDelayed()
    {
        // you can add here animation
        yield return new WaitForSeconds(_brokenPlatformConfig.DestroyDelay);
        DeInit();
        PoolObjectManager.instant.platformsPoolObjectManager.DisablePlatform(this, PlatformType);
    }
}
