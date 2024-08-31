using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static readonly float bombSize = 0.65f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();

        transform.localScale = Vector3.one * (bombSize / spriteRenderer.size.x);
    }
    public void Shoot(Vector2 velocity)
    {
        rb.velocity = velocity;
        rb.AddTorque(Mathf.Sign(Random.Range(-1f, 1f)) * 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BottomTrigger"))
        {
            gameManager.GameOver();
        }
        if (collision.CompareTag("GameOver"))
        {
            Destroy(gameObject);
        }
    }

}
