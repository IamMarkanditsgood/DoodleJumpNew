
using UnityEngine;

public class ReviveGui : MonoBehaviour
{
    public int maxRevives = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("RevivesCount"))
            PlayerPrefs.SetInt("RevivesCount", 0);
    }

    void OnEnable()
    {
        Revive();
    }

    //show revive popup
    void Revive()
    {
        GameManager.Instance.uIManager.gameState = GameState.REVIVE;
        AudioManager.Instance.PlayEffects(AudioManager.Instance.gameOver);

        if (!(PlayerPrefs.GetInt(Constants.REVIVES_COUNT, 0) < maxRevives))
        {
            PlayerPrefs.SetInt(Constants.REVIVES_COUNT, 0);
            GameOver();
        }
    }

    //trigger game over
    public void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    //player click on revive
    public void ContinueGame()
    {
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        PlayerPrefs.SetInt("RevivesCount", PlayerPrefs.GetInt("RevivesCount", 0) + 1);
        GameManager.Instance.ContinueGame();
        GameManager.Instance.uIManager.HideRevive();
    }
}
