using System;
using UnityEngine;

public class Customizer : MonoBehaviour, ICustomizer
{
    [Serializable]
    public class StyleData
    {
        public StyleTypes styleType;
        public Sprite styleSprite;
    }

    [SerializeField] protected StyleData[] _styles;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public virtual void Customize()
    {
        StyleTypes style = SaveManager.PlayerPrefs.LoadEnum(GameSaveKeys.GameStyle, StyleTypes.Default);
        SetStyle(style);
    }

    public void SetStyle(StyleTypes styleType)
    {
        if(styleType != StyleTypes.Default)
        {
            StyleData style = GetStyleData(styleType);
            _spriteRenderer.sprite = style.styleSprite;
        }
    }

    private StyleData GetStyleData(StyleTypes styleType)
    {
        foreach (StyleData s in _styles)
        {
            if(s.styleType == styleType)
            {
                return s;
            }
        }
        return null;
    }

   
}
