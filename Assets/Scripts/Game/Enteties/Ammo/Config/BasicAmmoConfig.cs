using UnityEngine;

public class BasicAmmoConfig : ScriptableObject
{
    [SerializeField] private AmmoTypes _ammoType;
    [Tooltip("Time after what object will be switched off")]
    [SerializeField] private float _lifeTime;

    public AmmoTypes AmmoType => _ammoType;
    public float LifeTime => _lifeTime;
}