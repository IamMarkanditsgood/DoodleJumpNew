using System;
using UnityEngine;

public class BulletController : BasicAmmoController
{
    private DefaultBulletConfig defaultBulletConfig;

    public override void Init(BasicAmmoConfig ammoConfig)
    {
        base.Init(ammoConfig);

        defaultBulletConfig = ammoConfig as DefaultBulletConfig;

        if (defaultBulletConfig == null)
        {
            throw new InvalidCastException($"Expected DefaultBulletConfig but got {ammoConfig.GetType().Name}");
        }
    }

    public override void UpdateAmmo()
    {
        base.UpdateAmmo();
        MoveBullet();
    }

    private void MoveBullet()
    {
        Vector2 moveDir = new Vector2(defaultBulletConfig.MovementDirection.x, defaultBulletConfig.MovementDirection.y).normalized;
        transform.Translate(moveDir * defaultBulletConfig.Speed * Time.deltaTime);
    }
}
