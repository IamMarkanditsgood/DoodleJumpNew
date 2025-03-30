using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[Header("GUI Components")]
	public GameObject mainMenuGui;
	public GameObject pauseGui, gameplayGui, gameOverGui, skinsGui, reviveGUI;

	[Header("GUI Animator")]
    public Animator guiAnimator;

    [Space(15)]
	public GameState gameState;

	[Header("Animated Text")]
	public Animator animatedTextAnimator;
	public Text animatedText;

	GameState previousState;
	bool clicked;

	// Use this for initialization
	void Start () {
		mainMenuGui.SetActive(true);
		pauseGui.SetActive(false);
		gameplayGui.SetActive(false);
		gameOverGui.SetActive(false);
		gameState = GameState.MENU;
	}

    void Update()
    {
		if (Input.GetMouseButtonDown(0) && gameState == GameState.MENU && !clicked)
		{
			if (IsButton())
				return;

			AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
			ShowGameplay();
		}
		else if (Input.GetMouseButtonUp(0) && clicked && gameState == GameState.MENU)
			clicked = false;
	}

    //show main menu
    public void ShowMainMenu()
	{
		StartCoroutine(MainMenu());
	}

	IEnumerator MainMenu()
	{
		if (gameState == GameState.PAUSED)
			Time.timeScale = 1;

		ScoreManager.Instance.ResetCurrentScore();
		clicked = true;

		ShowGameOverlay();

		gameState = GameState.MENU;
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);

		yield return new WaitForSecondsRealtime(.5f);

		GameManager.Instance.ClearScene();

		mainMenuGui.SetActive(true);
		pauseGui.SetActive(false);
		gameplayGui.SetActive(false);
		gameOverGui.SetActive(false);

		GameManager.Instance.NewGame();
		yield return new WaitForSecondsRealtime(.5f);


		AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
	}

    //show pause menu
    public void ShowPauseMenu()
	{
		if (gameState == GameState.PAUSED || gameState == GameState.REVIVE)
			return;

		GameManager.Instance.player.GetComponent<Player>().PlayerPause(true);
		guiAnimator.Play("ShowPause");
		Time.timeScale = 0;
		gameState = GameState.PAUSED;
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
	}

	//hide pause menu
	public void HidePauseMenu()
	{
		GameManager.Instance.player.GetComponent<Player>().PlayerPause(false);
		guiAnimator.Play("HidePause");
		Time.timeScale = 1;
		gameState = GameState.PLAYING;
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
	}

	//show gameplay gui
	public void ShowGameplay()
	{
		mainMenuGui.SetActive(false);
		pauseGui.SetActive(false);
		gameplayGui.SetActive(true);
		gameOverGui.SetActive(false);
		gameState = GameState.PLAYING;
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
		AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
	}

	//show skins gui
	public void ShowSkinsGui()
	{
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
		guiAnimator.Play("ShowSkins");
		previousState = gameState;
		gameState = GameState.POPUP;
	}

	//hide skins gui
	public void HideSkinsGui()
	{
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
		guiAnimator.Play("HideSkins");
		gameState = previousState;
	}

	//show game over gui
	public void ShowGameOver()
	{
		mainMenuGui.SetActive(false);
		pauseGui.SetActive(false);
		gameplayGui.SetActive(false);
		gameOverGui.SetActive(true);
		gameState = GameState.GAMEOVER;
		AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
	}

	//play game overlay animation
	public void ShowGameOverlay()
	{
		guiAnimator.Play("PlayOverlay", -1, 0.0f);
	}

    //show settings gui
	public void ShowSettings()
	{
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
		guiAnimator.Play("ShowSettings");
		previousState = gameState;
		gameState = GameState.POPUP;
    }

    //hide settings gui
	public void HideSettings()
	{
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
		guiAnimator.Play("HideSettings");
		gameState = previousState;
    }

	//use this method to show animated text
	public void ShowAnimatedText(string text) {
		animatedText.text = text;
		animatedTextAnimator.Play("Show");
    }

	//show revive gui
	public void ShowRevive()
	{
		reviveGUI.SetActive(true);
    }

	//hide revive gui
	public void HideRevive()
	{
		reviveGUI.SetActive(false);
	}

	//check if user click any menu button
	public bool IsButton()
	{
		bool temp = false;

		PointerEventData eventData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, results);

		foreach (RaycastResult item in results)
		{
			temp |= item.gameObject.GetComponent<Button>() != null;
		}

		return temp;
	}
}
