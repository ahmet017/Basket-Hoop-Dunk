using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hoop : MonoBehaviour
{
    public static readonly float hoopWidth = 1f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector2 spawnPosition;
    private float initialScale;

    void Start()
    {
        transform.position = spawnPosition;
        transform.localScale = Vector3.zero;
        initialScale = hoopWidth / spriteRenderer.size.x;
        StartCoroutine(SpawnHoop());
    }

    public void DestoryHoop()
    {
        StartCoroutine(Destroy(0, () => { }));
    }

    private IEnumerator Destroy(float delay, UnityAction onComplete)
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0;
        while (transform.localScale.x > 0)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(initialScale, 0, elapsedTime / 0.2f);
            yield return null;
        }
        onComplete.Invoke();
        Destroy(gameObject);
    }
    private IEnumerator SpawnHoop()
    {
        float elapsedTime = 0;
        while (transform.localScale.x < initialScale)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(0, initialScale, elapsedTime / 0.2f);
            yield return null;
        }
    }
}
