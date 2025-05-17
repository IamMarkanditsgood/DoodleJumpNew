using UnityEngine;

public class GameTags : MonoBehaviour
{
    [SerializeField, TagSelector]
    private string _playerTag = "Player";
    [SerializeField, TagSelector]
    private string _enemyTag = "Enemy";
    [SerializeField, TagSelector]
    private string _platformTag = "Platform";

    public string PlayerTag => _playerTag;
    public string EnemyTag => _enemyTag;
    public string PlatformTag => _platformTag;

    public static GameTags instantiate;

    private void Awake()
    {
        if (instantiate == null)
            instantiate = this;
    }
}
