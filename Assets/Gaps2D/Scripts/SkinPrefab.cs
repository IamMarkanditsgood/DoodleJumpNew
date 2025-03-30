using UnityEngine;
using UnityEngine.UI;

public class SkinPrefab : MonoBehaviour
{
    public Image skinImage;
    public Text buttonText;
    public GameObject coinImage;

    int skinIndex;
    bool locked;

    //set skin in the shop
    public void SetSkin(int _skinIndex, Sprite skinSprite)
    {
        skinImage.sprite = skinSprite;
        skinIndex = _skinIndex;

        CheckUnlocked();
    }

    //check if skin is unlocked
    public void CheckUnlocked()
    {
        if (PlayerPrefs.GetInt(Constants.SKIN_UNLOCKED + "_" + skinIndex, 0) == 0)
        {
            locked = true;
            skinImage.color = new Color(skinImage.color.r, skinImage.color.g, skinImage.color.b, .3f);
        }
        else
        {
            locked = false;
            skinImage.color = new Color(skinImage.color.r, skinImage.color.g, skinImage.color.b, 1f);
        }

        UpdateButtonText();
    }

    //update text on the button, depends if skin is unlocked, selected or locked
    void UpdateButtonText()
    {
        if (PlayerPrefs.GetInt(Constants.SKIN_SELECTED, 0) == skinIndex) //selected
        {
            buttonText.text = "Selected";
            coinImage.SetActive(false);
        }
        else if (PlayerPrefs.GetInt(Constants.SKIN_UNLOCKED + "_" + skinIndex, 0) == 0) //locked
        {
            buttonText.text = "" + SkinManager.Instance.skinPrice;
            coinImage.SetActive(true);
        }
        else //unlocked but not selected
        {
            buttonText.text = "Select";
            coinImage.SetActive(false);
        }
    }

    //return true if skin is unlocked
    public bool IsSkinUnlocked()
    {
        return !locked;
    }

    //click on skin
    public void SkinClicked()
    {
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);

        if (PlayerPrefs.GetInt(Constants.SKIN_SELECTED, 0) == skinIndex)
            GameManager.Instance.uIManager.HideSkinsGui();
        else if (locked)
        {
            if (CoinManager.Instance.CanSpendCoins(SkinManager.Instance.skinPrice))
            {
                CoinManager.Instance.SpendCoins(SkinManager.Instance.skinPrice);
                PlayerPrefs.SetInt(Constants.SKIN_SELECTED, skinIndex);
                GameManager.Instance.player.GetComponent<Player>().SetSkin(SkinManager.Instance.playerSkins[skinIndex]);
                PlayerPrefs.SetInt(Constants.SKIN_UNLOCKED + "_" + skinIndex, 1);
                locked = false;
            }
            else
            {
                AudioManager.Instance.PlayEffects(AudioManager.Instance.noMoney);
            }

        }
        else
        {
            PlayerPrefs.SetInt(Constants.SKIN_SELECTED, skinIndex);
            GameManager.Instance.player.GetComponent<Player>().SetSkin(SkinManager.Instance.playerSkins[skinIndex]);
        }

        SkinManager.Instance.RefreshSkins();
    }
}
