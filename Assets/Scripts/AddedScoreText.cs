using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddedScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textView;
    public Color color;
    public Color Color { get { return color; } set { color = value; } }

    public int Score
    {
        set
        {
            textView.text = "+" + value;
        }
    }
    public TMPro.TMP_FontAsset Font
    {
        set
        {
            textView.font = value;
        }
    }
    private void Awake()
    {
        textView.color = color;
    }

    private Vector2 currentPosition;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private IEnumerator Start()
    {
        startPosition = transform.position;
        currentPosition = transform.position;
        targetPosition = startPosition + Vector2.up * 1;

        float elapsedTime = 0;
        while (currentPosition != targetPosition)
        {
            elapsedTime += Time.deltaTime;
            currentPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / 1f);
            transform.position = currentPosition;
            yield return null;
        }

        Destroy(gameObject);
    }
}
