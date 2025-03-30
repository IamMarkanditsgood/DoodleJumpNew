using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance = null;

    [Header ("Audio Sources")]
    public AudioSource efxSource;
    public AudioSource musicSource;

    [Header ("Background Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip gameOver;
    public AudioClip crash;
    public AudioClip pickCoin;
    public AudioClip addCoins;
    public AudioClip hitCircle;
    public AudioClip noMoney;
    public AudioClip revive;

    bool muteMusic;
    bool muteEfx;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start ()
    {
        muteMusic = PlayerPrefs.GetInt(Constants.MUTE_MUSIC) == 1 ? true : false;
        muteEfx = PlayerPrefs.GetInt(Constants.MUTE_EFX) == 1 ? true : false;

        PlayMusic(menuMusic);
    }
	
    public void PlayMusic(AudioClip clip)
    {
        if (muteMusic)
            return;

        musicSource.clip = clip;
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    private void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayEffects(AudioClip clip)
    {
        if (muteEfx)
            return;
        
        efxSource.PlayOneShot(clip);
    }

    public void MuteMusic()
    {
        if (muteMusic)
        {
            muteMusic = false;
            PlayMusic(menuMusic);
            PlayerPrefs.SetInt(Constants.MUTE_MUSIC, 0);
        }
        else
        {
            muteMusic = true;
            StopMusic();
            PlayerPrefs.SetInt(Constants.MUTE_MUSIC, 1);
        }
    }

    public void MuteEfx()
    {
        if (muteEfx)
            PlayerPrefs.SetInt(Constants.MUTE_EFX, 0);
        else
            PlayerPrefs.SetInt(Constants.MUTE_EFX, 1);

        muteEfx = !muteEfx;
    }

    public bool IsMusicMute()
    {
        return muteMusic;
    }

    public bool IsEfxMute()
    {
        return muteEfx;
    }
}
