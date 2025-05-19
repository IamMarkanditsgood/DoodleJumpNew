using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimedBoosterConfig", menuName = "ScriptableObjects/GamePlayManager/Enteties/Booster/TimedBoosterConfig", order = 1)]
public class TimedBoosterConfig : BasicBoosterConfig
{
    [SerializeField] private float _timeOfBoostUse;

    public float TimeOfBoostUse => _timeOfBoostUse;
}
