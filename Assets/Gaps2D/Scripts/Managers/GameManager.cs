using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public UIManager uIManager;

    [Header("Game settings")]
    [Space(5)]
    //public Vector2 startGravity = new Vector2(0, -9.81f);
    [Space(5)]
    public Vector2 obstacleRotationSpeed = new Vector2(10, 50); //both values need to be more than zero so that obstacle is spinning (if obstacle wont spin player will be stuck)
    [Space(5)]
    public GameObject player;
    public Rigidbody2D playerRigidBody2D;
    public int playerJumpSpeed = 5;
    public float xObstacleOffset = 1;
    [Space(5)]
    public Color[] colorTable;
    [Space(5)]
    public GameObject[] obstaclePrefabs;
    [Space(5)]
    public GameObject doorsPrefab;
    [Space(5)]
    public int distanceBetweenObjects = 6;
    [Space(5)]
    public Transform camObject;

    bool started;
    GameObject lastObstacle;
    GameObject lastDoors;
    public Vector2 screenSize;
    bool inAir;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.gravity = startGravity;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Application.targetFrameRate = 60;

        player.GetComponent<Player>().SetSkin(SkinManager.Instance.playerSkins[PlayerPrefs.GetInt(Constants.SKIN_SELECTED, 0)]);

        NewGame();
    }

    void FixedUpdate()
    {
        if (uIManager.gameState == GameState.PLAYING && Input.GetMouseButton(0))
        {
            if (uIManager.IsButton())
                return;

            if (started && !inAir)
                Shoot();

            if (!started)
                started = true;

        }
    }

    //restart game, reset score, update platform position
    public void RestartGame()
    {
        if (uIManager.gameState == GameState.PAUSED)
        {
            player.SetActive(false);
            uIManager.HidePauseMenu();
        }

        ScoreManager.Instance.ResetCurrentScore();

        ClearScene();
        NewGame();
        uIManager.ShowGameplay();
    }

    //create new game
    public void NewGame()
    {
        //create first obstacle
        lastObstacle = Instantiate(RandomObstacle());
        lastObstacle.transform.position = Vector2.zero;
        lastObstacle.transform.GetChild(0).GetComponent<Obstacle>().Init(camObject, GetRandomColor(), Random.Range(obstacleRotationSpeed.x, obstacleRotationSpeed.y));

        NewObstacle();
        NewObstacle();
        NewObstacle();
        NewObstacle();

        //move player bellow the screen and shot it towards the first obstacle
        player.SetActive(true);
        player.transform.position = new Vector2(0, -screenSize.y - 1);
        playerRigidBody2D.AddForce(transform.up * playerJumpSpeed/2, ForceMode2D.Impulse);
        player.GetComponent<Player>().Reset();
        inAir = true;

        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
    }

    //create new obstacle on position
    public void NewObstacle()
    {
        GameObject tempObstacle = lastObstacle;

        lastObstacle = Instantiate(RandomObstacle());
        lastObstacle.transform.position = new Vector2(Random.Range(-xObstacleOffset, xObstacleOffset), tempObstacle.transform.position.y + distanceBetweenObjects);
        lastObstacle.transform.GetChild(0).GetComponent<Obstacle>().Init(camObject, GetRandomColor(), Random.Range(obstacleRotationSpeed.x, obstacleRotationSpeed.y));

        NewDoors(tempObstacle.transform);
    }

    //create new doors
    public void NewDoors(Transform previousObstaclePos)
    {
        lastDoors = Instantiate(doorsPrefab);
        lastDoors.transform.position = new Vector2((previousObstaclePos.position.x + lastObstacle.transform.position.x) / 2, lastObstacle.transform.position.y - distanceBetweenObjects / 2);
        lastDoors.GetComponent<Doors>().Init(camObject, GetRandomColor());
    }

    //player hit obstacle
    public void HitObstacle()
    {
        inAir = false;

        if (uIManager.gameState == GameState.PLAYING)
            ScoreManager.Instance.UpdateScore(1);
    }

    //return random obstacle from obstacle array
    GameObject RandomObstacle()
    {
        return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
    }

    //shoot the player
    public void Shoot()
    {
        player.GetComponent<Player>().Shoot();
        inAir = true;
    }

    //continue current game
    public void ContinueGame()
    {
        player.SetActive(true);
        player.GetComponent<Player>().Revive();
    }

    //clear all blocks from scene
    public void ClearScene()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        //detach player from any obstacle (otherwise it will get destroyed)
        player.transform.parent = null;

        foreach (GameObject item in obstacles)
        {
            Destroy(item);
        }

        camObject.GetComponent<CameraFollowTarget>().ResetCameraPosition();
    }

    //get random color from color table
    public Color GetRandomColor()
    {
        return colorTable[Random.Range(0, colorTable.Length)];
    }

    //show game over gui
    public void GameOver()
    {
        if (uIManager.gameState == GameState.PLAYING || uIManager.gameState == GameState.REVIVE)
        {
            if (uIManager.reviveGUI.activeInHierarchy)
                uIManager.reviveGUI.SetActive(false);

            player.SetActive(false);
            uIManager.ShowGameOver();
            ScoreManager.Instance.UpdateScoreGameover();
        }
    }
}
