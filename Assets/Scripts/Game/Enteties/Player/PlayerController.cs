using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public float deathDistanceBelowCamera = 5f; // ��������� ���� ������ ����� ������

    private Rigidbody2D rb;
    private float horizontalInput;
    private float screenWidthInUnits;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ��������� ������ ������ � ������� �����������
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        screenWidthInUnits = screenWidth / 2f;
    }

    void Update()
    {
        // �������������� ��������
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // ����� �� ����������� (wrap)
        if (transform.position.x > screenWidthInUnits)
            transform.position = new Vector2(-screenWidthInUnits, transform.position.y);
        else if (transform.position.x < -screenWidthInUnits)
            transform.position = new Vector2(screenWidthInUnits, transform.position.y);

        // ��������: �� ���� �� ����� ������� �����
        float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        if (transform.position.y < cameraBottom - deathDistanceBelowCamera)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            float playerBottom = transform.position.y - GetComponent<Collider2D>().bounds.extents.y;
            float platformTop = collision.collider.bounds.center.y + collision.collider.bounds.extents.y;

            if (playerBottom > platformTop - 0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        Time.timeScale = 0;
        // ��� ������ �������� ���������� �����, �������� ������ � �.�.
        // ��������:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
