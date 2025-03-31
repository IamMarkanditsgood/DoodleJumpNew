using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int numberOfPlatforms = 10;
    public float minY = 0.5f;
    public float maxY = 2.5f;

    void Start()
    {
        float yPosition = 0f;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            float xPosition = Random.Range(-2.5f, 2.5f);
            yPosition += Random.Range(minY, maxY);
            Instantiate(platformPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        }
    }
}
