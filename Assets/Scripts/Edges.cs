using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edges : MonoBehaviour
{
    private Vector2 screenBounds; 

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        EdgeCollider2D leftEdge = gameObject.AddComponent<EdgeCollider2D>();
        EdgeCollider2D rightEdge = gameObject.AddComponent<EdgeCollider2D>();

        Vector2[] leftPoints = new Vector2[2];
        Vector2[] rightPoints = new Vector2[2];

        leftPoints[0] = new Vector2(-screenBounds.x, screenBounds.y * 2);
        leftPoints[1] = new Vector2(-screenBounds.x, -screenBounds.y * 2);

        rightPoints[0] = new Vector2(screenBounds.x, screenBounds.y * 2);
        rightPoints[1] = new Vector2(screenBounds.x, -screenBounds.y * 2);

        leftEdge.points = leftPoints;
        rightEdge.points = rightPoints;
    }

}
