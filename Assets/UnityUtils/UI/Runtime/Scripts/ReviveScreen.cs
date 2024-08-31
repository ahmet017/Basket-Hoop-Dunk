using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UnityUtils.UI
{
    public class ReviveScreen : MonoBehaviour
    {
        public enum CircleStyleEnum { Bold, Normal }

        [SerializeField] private Image circle;
        [SerializeField] private Image progressCircle;
        [SerializeField] private Image background;
        [SerializeField] private Button reviveButton;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Sprite boldSprite;
        [SerializeField] private Sprite normalSprite;

        [Header("Settings")]
        [SerializeField] private float duration = 6f;
        [SerializeField] private CircleStyleEnum circleStyle;
        [SerializeField] private Color circleFillColor;
        [SerializeField] private Color circleEmptyColor;
        [SerializeField] private Color backgroundColor;

        public CircleStyleEnum CircleStyle
        {
            get
            {
                return circleStyle;
            }
            set
            {
                circleStyle = value;
                if (circleStyle == CircleStyleEnum.Normal)
                {
                    circle.sprite = normalSprite;
                    progressCircle.sprite = normalSprite;
                }
                else
                {
                    circle.sprite = boldSprite;
                    progressCircle.sprite = boldSprite;
                }
            }
        }
        public Color FillColor
        {
            get
            {
                return circleFillColor;
            }
            set
            {
                circleFillColor = value;
                progressCircle.color = circleFillColor;
                timerText.color = circleFillColor;
            }
        }
        public Color EmptyColor
        {
            get
            {
                return circleEmptyColor;
            }
            set
            {
                circleEmptyColor = value;
                circle.color = circleEmptyColor;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return BackgroundColor;
            }
            set
            {
                backgroundColor = value;
                background.color = backgroundColor;
            }
        }

        private float elapsedTime;
        private UnityAction<bool> callback;

        void Update()
        {
            elapsedTime += Time.unscaledDeltaTime;
            progressCircle.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            timerText.text = Mathf.Ceil(progressCircle.fillAmount * duration).ToString();

            if (progressCircle.fillAmount == 0)
            {
                gameObject.SetActive(false);
                if (callback != null)
                    callback.Invoke(false);
            }
        }

        public void Show(UnityAction<bool> callback)
        {
            elapsedTime = 0;
            this.callback = callback;
            reviveButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                callback.Invoke(true);
            });
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (circleStyle == CircleStyleEnum.Normal)
            {
                circle.sprite = normalSprite;
                progressCircle.sprite = normalSprite;
            }
            else
            {
                circle.sprite = boldSprite;
                progressCircle.sprite = boldSprite;
            }

            circle.color = circleEmptyColor;
            progressCircle.color = circleFillColor;
            background.color = backgroundColor;
            timerText.color = circleFillColor;
        }
#endif
    }
}
