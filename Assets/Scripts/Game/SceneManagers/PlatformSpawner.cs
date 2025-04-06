using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Префаб")]
    public GameObject platformPrefab;

    [Header("Параметры спавна")]
    public float spawnIntervalY = 2f;
    public int platformsAhead = 10;
    public Vector2 platformSpacingXRange = new Vector2(2f, 4f);

    [Header("Границы спавна по X")]
    public bool useManualXBounds = false;

    [SerializeField] private float manualMinX = -5f;
    [SerializeField] private float manualMaxX = 5f;

    private float lastY = 0f;
    private Camera mainCamera;
    private List<Vector2> spawnedPositions = new List<Vector2>();

    void Start()
    {
        mainCamera = Camera.main;
        lastY = transform.position.y;
    }

    void Update()
    {
        float topOfCamera = mainCamera.transform.position.y + mainCamera.orthographicSize;

        while (lastY < topOfCamera + platformsAhead * spawnIntervalY)
        {
            SpawnPlatformRow();
            lastY += spawnIntervalY;
        }
    }

    void SpawnPlatformRow()
    {
        int attempts = 0;
        bool placed = false;

        float minX, maxX;

        if (useManualXBounds)
        {
            minX = manualMinX;
            maxX = manualMaxX;
        }
        else
        {
            float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
            minX = -screenWidth + 0.5f;
            maxX = screenWidth - 0.5f;
        }

        while (!placed && attempts < 15)
        {
            attempts++;

            float x = Random.Range(minX, maxX);
            Vector2 spawnPos = new Vector2(x, lastY);
            float requiredSpacing = Random.Range(platformSpacingXRange.x, platformSpacingXRange.y);

            if (IsPositionFree(spawnPos, requiredSpacing))
            {
                Instantiate(platformPrefab, spawnPos, Quaternion.identity);
                spawnedPositions.Add(spawnPos);
                placed = true;
            }
        }
    }

    bool IsPositionFree(Vector2 pos, float minDistance)
    {
        foreach (Vector2 existing in spawnedPositions)
        {
            if (Vector2.Distance(existing, pos) < minDistance)
                return false;
        }
        return true;
    }
}
