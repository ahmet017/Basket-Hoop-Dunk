using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension 
{
    
    public static Vector3 GetWorldPosition(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return new Vector2((positions[0].x + positions[2].x) * 0.5f, (positions[1].y + positions[3].y) * 0.5f);
    }

    public static float GetWorldWidth(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return Vector2.Distance(positions[0], positions[3]);
    }

    public static float GetWorldHeight(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return Vector2.Distance(positions[0], positions[1]);
    }

    public static Vector2 GetBottomLeftCorner(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return positions[0];
    }
    public static Vector2 GetTopLeftCorner(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return positions[1];
    }
    public static Vector2 GetTopRightCorner(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return positions[2];
    }
    public static Vector2 GetBottomRightCorner(this RectTransform rectTransform)
    {
        Vector3[] positions = new Vector3[4];
        rectTransform.GetWorldCorners(positions);

        return positions[3];
    }


    // Offsets

    public static void SetLeft(this RectTransform rectTransform, float left)
    {
        rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
    }
    public static void SetRight(this RectTransform rectTransform, float right)
    {
        rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
    }
    public static void SetTop(this RectTransform rectTransform, float top)
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
    }
    public static void SetBottom(this RectTransform rectTransform, float bottom)
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
    }
    public static void ClearOffset(this RectTransform rectTransform)
    {
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
