﻿using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    public bool efx;
    public Sprite onSprite, offSprite;
    public Image spriteButton;


    //set button sprite
    void Start()
    {
        SetButton();
    }

    public void MusicButtonClicked()
    {
        AudioManager.Instance.MuteMusic();
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        SetButton();
    }

    public void EfxButtonClicked()
    {
        AudioManager.Instance.MuteEfx();
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        SetButton();
    }

    void SetButton()
    {
        if ((!AudioManager.Instance.IsMusicMute() && !efx) || (!AudioManager.Instance.IsEfxMute() && efx))
            spriteButton.sprite = onSprite;
        else
            spriteButton.sprite = offSprite;
    }
}
