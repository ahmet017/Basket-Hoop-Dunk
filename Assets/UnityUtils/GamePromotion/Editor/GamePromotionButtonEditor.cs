using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace UnityUtils.GamePromotion.Editor
{
    [CustomEditor(typeof(GamePromotionButton))]
    public class GamePromotionButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("OPEN EDITOR"))
            {
                GamePromotionManagerWindow.Open();
            }
        }
    }
}
#endif
