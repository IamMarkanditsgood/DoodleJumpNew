using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicAmmoController : MonoBehaviour
{
    private AmmoConfig _ammoConfig;

    private bool _isActive;   

    public virtual void Init(AmmoConfig ammoConfig)
    {
        _ammoConfig = ammoConfig;
    }

    public virtual void Toggle(bool state)
    {
        _isActive = state;
    }

    public virtual void Move() { if (!_isActive) return; }

    public virtual void HitObject(GameObject hitedObject)
    {
        IHitable hitablePlayer = hitedObject.GetComponent<IHitable>();

        if(hitablePlayer != null)
            hitablePlayer.Hit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive || !collision.gameObject.CompareTag(GameTags.instantiate.PlayerTag)) return;

        HitObject(collision.gameObject);
    }
}
