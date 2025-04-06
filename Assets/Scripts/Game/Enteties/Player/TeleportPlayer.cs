using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Получаем ссылку на основную камеру
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Получаем позицию игрока
        Vector3 playerPosition = transform.position;

        // Получаем размеры экрана в мировых координатах
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;

        // Проверяем, если игрок выходит за границы экрана по X (горизонтально)
        if (playerPosition.x > screenWidth)
        {
            playerPosition.x = -screenWidth; // Перемещаем на противоположную сторону
        }
        else if (playerPosition.x < -screenWidth)
        {
            playerPosition.x = screenWidth; // Перемещаем на противоположную сторону
        }

        // Обновляем позицию игрока
        transform.position = playerPosition;
    }
}
