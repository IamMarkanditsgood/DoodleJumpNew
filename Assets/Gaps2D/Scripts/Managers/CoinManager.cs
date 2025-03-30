using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {

    public static CoinManager Instance;

    public int initialCoins = 0; //number of initial coins
    public int coins; //total coins
    public Text coinsText; //coins label text on the scene

    float updateDuration = .3f; //coins update duration in seconds
    bool updatingCoins;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        coins = PlayerPrefs.GetInt(Constants.COINS, initialCoins);
        UpdateCoinsText();
    }

    //call this method when player pick a coin
    public void PickCoin(int amount)
    {
        AudioManager.Instance.PlayEffects(AudioManager.Instance.pickCoin);
        coins += amount;
        coinsText.text = coins.ToString();
        PlayerPrefs.SetInt(Constants.COINS, coins);
    }

    //update coins text
    void UpdateCoinsText()
    {
        coinsText.text = coins.ToString();
    }

    //call this method, when you need add coins through time
    public void AddCoins(int amount)
    {
        if (updatingCoins)
            return;
            
        StartCoroutine(UpdateCoins(amount, updateDuration));
        AudioManager.Instance.PlayEffects(AudioManager.Instance.addCoins);
    }

    //call this method, when player spend coins 
    public void SpendCoins(int amount)
    {
        if (updatingCoins)
            return;
            
        StartCoroutine(UpdateCoins(-amount, updateDuration));
        AudioManager.Instance.PlayEffects(AudioManager.Instance.addCoins);
    }

    //use this method to check if player can spent given amount of coins
    public bool CanSpendCoins(int amount)
    {
        return amount <= coins;
    }

    //use this method to update coins throught the time, for decreasing just set amount to negative value
    IEnumerator UpdateCoins(int amount, float time)
    {
        if (coinsText == null || updatingCoins)
        {
            yield return 0;
        }
        else
        {
            updatingCoins = true;

            int endCoins = coins + amount;

            //Debug.Log(amount);

            if (amount < 0)
            {
                float delta = -amount / (60 * time);

                while (coins > endCoins)
                {
                    coins = coins - Mathf.CeilToInt(delta);

                    if (coins < endCoins)
                        coins = endCoins;

                    coinsText.text = coins.ToString();

                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                float delta = amount / (60 * time);

                while (coins < endCoins)
                {
                    coins = coins + Mathf.CeilToInt(delta);

                    if (coins > endCoins)
                        coins = endCoins;

                    coinsText.text = coins.ToString();

                    yield return new WaitForFixedUpdate();
                }
            }

            updatingCoins = false;
            PlayerPrefs.SetInt(Constants.COINS, coins);
        }
    }
}
