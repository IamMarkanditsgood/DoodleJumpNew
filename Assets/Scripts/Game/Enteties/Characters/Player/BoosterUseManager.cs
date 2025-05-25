using System.Collections;
using UnityEngine;

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