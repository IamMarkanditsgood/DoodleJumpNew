using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance = null;

    [Space(5)]
    public PlayerSkin[] playerSkins;
    [Space(5)]
    public int skinPrice = 50;
    [Space(5)]
    public GameObject skinPrefab;
    [Space(5)]
    public Transform skinsParent;

    List<GameObject> skins = new List<GameObject>();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //unlock first skin
        if (!PlayerPrefs.HasKey(Constants.SKIN_UNLOCKED + "_0"))
            PlayerPrefs.SetInt(Constants.SKIN_UNLOCKED + "_0", 0);

        //set default selected skin
        if (!PlayerPrefs.HasKey(Constants.SKIN_SELECTED))
            PlayerPrefs.SetInt(Constants.SKIN_SELECTED, 0);

        InitSkins();
    }

    //create content for skins popup
    void InitSkins()
    {
        GameObject tempObject;

        for (int i = 0; i < playerSkins.Length; i++)
        {
            tempObject = Instantiate(skinPrefab, skinsParent);
            tempObject.GetComponent<SkinPrefab>().SetSkin(i, playerSkins[i].playerSprite);
            skins.Add(tempObject);

        }

        //init skins layout height
        //gridLayoutGroup.value = 0.0f;
    }

    public void RefreshSkins()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].GetComponent<SkinPrefab>().CheckUnlocked();
        }
    }
}
