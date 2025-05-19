using UnityEngine;

public class BasicBoosterController : MonoBehaviour
{
    protected BasicBoosterConfig basicBoosterConfig;
    protected bool isActive;

    public BasicBoosterConfig BoosterConfig => basicBoosterConfig;

    public virtual void Init(BasicBoosterConfig ñonfig)
    {
   
        basicBoosterConfig = ñonfig;
        Debug.Log(basicBoosterConfig.BoosterType);
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

    public virtual void Interact()
    {
        Toggle(false);
    }

    private void ConfigBooster()
    {
        gameObject.SetActive(true);
        gameObject.tag = basicBoosterConfig.BoostTag;
    }
}