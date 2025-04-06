using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // �������� ������ �� �������� ������
        mainCamera = Camera.main;
    }

    void Update()
    {
        // �������� ������� ������
        Vector3 playerPosition = transform.position;

        // �������� ������� ������ � ������� �����������
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;

        // ���������, ���� ����� ������� �� ������� ������ �� X (�������������)
        if (playerPosition.x > screenWidth)
        {
            playerPosition.x = -screenWidth; // ���������� �� ��������������� �������
        }
        else if (playerPosition.x < -screenWidth)
        {
            playerPosition.x = screenWidth; // ���������� �� ��������������� �������
        }

        // ��������� ������� ������
        transform.position = playerPosition;
    }
}
