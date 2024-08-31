using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FillOutsideSafeArea : MonoBehaviour
    {
        public enum FillPosition { Top, Bottom }

        [SerializeField] private RectTransform safeArea;
        [SerializeField] private FillPosition fillPosition;
        private SpriteRenderer spriteRenderer;
        private Vector2 spriteScale;
        private Vector2 screenBounds;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteScale = spriteRenderer.size;
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


            if (fillPosition == FillPosition.Top)
            {
                transform.localScale = new Vector3((screenBounds.x * 2) / spriteScale.x, (screenBounds.y - safeArea.GetTopLeftCorner().y) / spriteScale.y, 1);
                transform.position = new Vector2(0, (screenBounds.y + safeArea.GetTopLeftCorner().y) / 2);
            }
            else
            {
                transform.localScale = new Vector3((screenBounds.x * 2) / spriteScale.x, (safeArea.GetBottomLeftCorner().y - (-screenBounds.y)) / spriteScale.y, 1);
                transform.position = new Vector2(0, (-screenBounds.y + safeArea.GetBottomLeftCorner().y) / 2);
            }
        }

    }
}

