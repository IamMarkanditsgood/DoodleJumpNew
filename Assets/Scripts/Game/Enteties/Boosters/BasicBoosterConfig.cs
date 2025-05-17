using UnityEngine;

[CreateAssetMenu(fileName = "BasicBoosterConfig", menuName = "ScriptableObjects/GamePlayManager/Enteties/Booster/BasicBoosterConfig", order = 1)]
public class BasicBoosterConfig : ScriptableObject
{
    [SerializeField] private BooterTypes _booterType;
    [Range(0, 100)]
    [SerializeField] private float _usingChance;
    [SerializeField] private float _boostJumpForce;
    [SerializeField, TagSelector]
    private string _boosterTag;

    public BooterTypes BooterType => _booterType;
    public float UsingChance => _usingChance;
    public float BoostJumpForce => _boostJumpForce;
    public string BoostTag => _boosterTag;
}