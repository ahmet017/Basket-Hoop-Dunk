using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityUtils.UI
{
    [ExecuteInEditMode]
    public class RoundedButton : MonoBehaviour
    {
        [SerializeField] [Hidden] private Image foregroundImage;
        [SerializeField] [Hidden] private Image backgroundImage;
        [SerializeField] [Hidden] private Image outlineImage;
        [SerializeField] [Hidden] private RectTransform foregroundRect;
        [SerializeField] [Hidden] private RectTransform outlineRect;
        
        [Header("Settings")]
        [SerializeField] private Color color;
        [SerializeField] private float cornerRadius;

        [Header("Shadow")]
        [SerializeField] private bool shadowEnabled;
        [SerializeField] private float shadowDistance;
        [Range(0f, 1f)] public float shadowDarkness;
        [Header("Outline")]
        [SerializeField] private bool outlineEnabled;
        [SerializeField] private float outlineDistance;
        [SerializeField] private Color outlineColor;

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                foregroundImage.color = color;
                backgroundImage.color = color.SetValue(color.GetValue() * shadowDarkness);
            }
        }
        public float CornerRadius
        {
            get
            {
                return cornerRadius;
            }
            set
            {
                cornerRadius = value;
                foregroundImage.pixelsPerUnitMultiplier = cornerRadius;
                backgroundImage.pixelsPerUnitMultiplier = cornerRadius;
                outlineImage.pixelsPerUnitMultiplier = cornerRadius;
            }
        }
        public bool ShadowEnabled
        {
            get
            {
                return shadowEnabled;
            }
            set
            {
                shadowEnabled = value;
                backgroundImage.gameObject.SetActive(shadowEnabled);
                foregroundRect.offsetMin = shadowEnabled ? new Vector2(foregroundRect.offsetMin.x, shadowDistance) : Vector2.zero;
            }
        }
        public float ShadowDistance
        {
            get
            {
                return shadowDistance;
            }
            set
            {
                shadowDistance = value;
                foregroundRect.offsetMin = shadowEnabled ? new Vector2(foregroundRect.offsetMin.x, shadowDistance) : Vector2.zero;
            }
        } 
        public float ShadowDarkness
        {
            get
            {
                return shadowDarkness;
            }
            set
            {
                shadowDarkness = value;
                backgroundImage.color = color.SetValue(color.GetValue() * shadowDarkness);
            }
        }
        public bool OutlineEnabled
        {
            get
            {
                return outlineEnabled;
            }
            set
            {
                outlineEnabled = value;
                outlineImage.gameObject.SetActive(outlineEnabled);
            }
        }
        public float OutlineDistance
        {
            get
            {
                return outlineDistance;
            }
            set
            {
                outlineDistance = value;
                outlineRect.offsetMin = Vector2.one * -outlineDistance;
                outlineRect.offsetMax = Vector2.one * outlineDistance;
            }
        }
        public Color OutlineColor
        {
            get
            {
                return outlineColor;
            }
            set
            {
                outlineColor = value;
                outlineImage.color = outlineColor;
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (foregroundImage == null || backgroundImage == null || outlineImage == null
                || foregroundRect == null || outlineRect == null)
                return;
            
            foregroundImage.pixelsPerUnitMultiplier = cornerRadius;
            backgroundImage.pixelsPerUnitMultiplier = cornerRadius;
            outlineImage.pixelsPerUnitMultiplier = cornerRadius;

            backgroundImage.gameObject.SetActive(shadowEnabled);
            outlineImage.gameObject.SetActive(outlineEnabled);

            foregroundImage.color = color;
            backgroundImage.color = color.SetValue(color.GetValue() * shadowDarkness);

            outlineImage.color = outlineColor;

            EditorApplication.delayCall += _OnValidate;
        }

        private void _OnValidate()
        {
            if (foregroundRect != null)
            {
                foregroundRect.offsetMin = shadowEnabled ? new Vector2(foregroundRect.offsetMin.x, shadowDistance) : Vector2.zero;
            }
            if (outlineRect != null)
            {
                outlineRect.offsetMin = Vector2.one * -outlineDistance;
                outlineRect.offsetMax = Vector2.one * outlineDistance;
            }
            EditorApplication.delayCall -= _OnValidate;
        }
#endif
    }


}
