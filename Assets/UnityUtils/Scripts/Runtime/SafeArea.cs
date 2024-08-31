using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    public class SafeArea : MonoBehaviour
    {
        RectTransform rectTransform;
        Rect safeArea;

        Vector2 minAnchor;
        Vector2 maxAnchor;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            safeArea = Screen.safeArea;
            minAnchor = safeArea.position;
            maxAnchor = minAnchor + safeArea.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;

            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rectTransform.anchorMin = minAnchor + new Vector2(0, 40 / Screen.height);
            rectTransform.anchorMax = maxAnchor;
        }

    }
}
