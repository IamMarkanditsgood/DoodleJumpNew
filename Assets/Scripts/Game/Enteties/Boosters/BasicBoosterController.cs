using System;
using UnityEngine;

public class BasicBoosterController : MonoBehaviour
{
    [Serializable]
    public class StyleData
    {
        public StyleTypes styleType;
        public Sprite styleSprite;
    }
    [Serializable]
    public class BossterCustimizerData
    {
        public StyleData[] _styles;
        [SerializeField, TagSelector]
        public string _boosterTag;

    }

    [SerializeField] private BossterCustimizerData[] _boosterStyles;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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
        Customize();
    }

    public void Customize()
    {
        foreach(var boosterStyle in _boosterStyles)
        {
            if(gameObject.tag == boosterStyle._boosterTag)
            {
                StyleTypes style = SaveManager.PlayerPrefs.LoadEnum(GameSaveKeys.GameStyle, StyleTypes.Default);
                SetStyle(style, boosterStyle._styles);
            }
        }  
    }

    public void SetStyle(StyleTypes styleType, StyleData[] styleData)
    {
        if (styleType != StyleTypes.Default)
        {
            StyleData style = GetStyleData(styleType, styleData);
            _spriteRenderer.sprite = style.styleSprite;
        }
    }

    private StyleData GetStyleData(StyleTypes styleType, StyleData[] style)
    {
        foreach (StyleData s in style)
        {
            if (s.styleType == styleType)
            {
                return s;
            }
        }
        return null;
    }
}