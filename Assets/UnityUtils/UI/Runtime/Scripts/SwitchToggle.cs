using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UnityUtils.UI
{
    [ExecuteInEditMode]
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] [Hidden] private Toggle toggle;
        [SerializeField] [Hidden] private RectTransform circle;
        [SerializeField] [Hidden] private Image background;

        [SerializeField] private Color colorOn, colorOff;
        public bool IsOn { get { return toggle.isOn; } set { toggle.isOn = value; } }
        public UnityEvent<bool> onValueChanged;

        void Awake()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                SetSelected(toggle.isOn);
                onValueChanged?.Invoke(isOn);
            });
        }

        private void SetSelected(bool isSelected)
        {
            circle.pivot = new Vector2(isSelected ? 1 : 0, 0.5f);
            circle.anchorMin = new Vector2(isSelected ? 0.95f : 0.05f, 0);
            circle.anchorMax = new Vector2(isSelected ? 0.95f : 0.05f, 1);
            background.color = isSelected ? colorOn : colorOff;
        }
    }
}


