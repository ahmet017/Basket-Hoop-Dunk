using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static readonly float ballSize = 0.5f;
    public float Speed { get { return rb.velocity.magnitude; } }

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private SkinManager skinManager;
    private CircleCollider2D circleCollider;
    private AudioManager audioManager;
    private bool touchedToHoop;
    private bool topTriggered;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        skinManager = FindObjectOfType<SkinManager>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioManager = FindObjectOfType<AudioManager>();
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = skinManager.GetSelectedBallSkin().sprite;
        spriteRenderer.sortingOrder = 20;
        rb.isKinematic = true;

        transform.localScale = Vector3.one * (ballSize / spriteRenderer.size.x);
        circleCollider.radius = spriteRenderer.size.x * 0.5f;
    }


    public void Shoot(Vector2 velocity)
    {
        rb.isKinematic = false;
        rb.velocity = velocity;
    }
    private IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopTrigger"))
        {
            topTriggered = true;
        }
        if (collision.CompareTag("BottomTrigger"))
        {
            if (topTriggered)
            {
                gameManager.Dunk(transform.position, touchedToHoop);
                StartCoroutine(DestroyBall());
            }
        }
        if (collision.CompareTag("GameOver"))
        {
            gameManager.GameOver();
        }
    }

    private float magnitude;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        magnitude = collision.relativeVelocity.magnitude;
        audioManager.BallHit(magnitude / 10f);

        if (collision.collider.CompareTag("HoopCollider"))
            touchedToHoop = true;
    }
}
