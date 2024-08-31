using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textView;
    public Color textColor;
    public Color animationColor;
    public Color TextColor { get { return textColor; } set { textColor = value; } }
    public Color AnimationColor { get { return animationColor; } set { animationColor = value; } }

    private Coroutine coroutine;
    private float initialScale;
    private float targetScale;

    private void Awake()
    {
        initialScale = transform.localScale.x;
        targetScale = initialScale * 1.2f;
    }

    public void ScoreUpdated()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(Animate());
    }

    private readonly float duration = 0.5f;
    private float halfDuration;
    private float elapsedTime;
   
    private IEnumerator Animate()
    {
        halfDuration = duration * 0.5f;
        elapsedTime = 0;

        while (transform.localScale.x != targetScale)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(initialScale, targetScale, elapsedTime / halfDuration);
            textView.color = Color.Lerp(textColor, animationColor, elapsedTime / halfDuration);
            yield return null;
        }
        elapsedTime = 0;
        while (transform.localScale.x != initialScale)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(targetScale, initialScale, elapsedTime / halfDuration);
            textView.color = Color.Lerp(animationColor, textColor, elapsedTime / halfDuration);
            yield return null;
        }
    }


}
