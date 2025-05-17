using UnityEngine;

public class BasicBoosterController : MonoBehaviour
{
    protected BasicBoosterConfig basicBoosterConfig;
    protected bool isActive;

    public virtual void Init(BasicBoosterConfig ñonfig)
    {
        basicBoosterConfig = ñonfig;
    }

    public virtual void Toggle(bool state)
    {
        isActive = state;

        if (isActive)
        {
            ConfigBooster();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public float Interact()
    {
        Toggle(false);
        return basicBoosterConfig.BoostJumpForce;
    }

    private void ConfigBooster()
    {
        gameObject.SetActive(true);
        gameObject.tag = basicBoosterConfig.BoostTag;
    }
}