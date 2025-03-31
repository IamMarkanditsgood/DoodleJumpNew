using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Wrap-around movement (teleport player when reaching the screen edge)
        if (transform.position.x > 2.5f)
            transform.position = new Vector3(-2.5f, transform.position.y, transform.position.z);
        else if (transform.position.x < -2.5f)
            transform.position = new Vector3(2.5f, transform.position.y, transform.position.z);
    }
}
