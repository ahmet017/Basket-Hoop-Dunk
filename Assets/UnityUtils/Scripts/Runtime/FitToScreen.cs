using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FitToScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform rectToFit;
        public bool keepAspectRatio;


        void Start()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            Sprite sprite = Sprite.Create(renderer.sprite.texture, renderer.sprite.rect, new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);

            renderer.sprite = sprite;
            renderer.drawMode = SpriteDrawMode.Sliced;

            float fitX, fitY;
            Vector2 fitCenter;
            if (rectToFit != null)
            {
                fitX = rectToFit.GetWorldWidth();
                fitY = rectToFit.GetWorldHeight();
                fitCenter = rectToFit.GetWorldPosition();
            }
            else
            {
                fitX = screenBounds.x * 2;
                fitY = screenBounds.y * 2;
                fitCenter = Vector2.zero;
            }

            if (!keepAspectRatio)
            {
                renderer.size = new Vector2(fitX, fitY);
            }
            else
            {
                float width = sprite.texture.width;
                float height = sprite.texture.height;
                float whRatio = width / height;
                float hwRatio = height / width;

                if (fitX * hwRatio < fitY)
                {
                    renderer.size = new Vector2(fitY * whRatio, fitY);
                }
                else
                {
                    renderer.size = new Vector2(fitX, fitX * hwRatio);
                }
            }

            transform.localScale = Vector2.one;
            transform.position = fitCenter;

        }


    }

}
