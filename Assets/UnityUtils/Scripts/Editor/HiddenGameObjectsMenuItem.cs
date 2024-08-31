using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace UnityUtils.Editor
{
    public class HiddenGameObjectsMenuItem
    {
        const string menu_name = "Show Hidden Objects and Fields";

        [MenuItem("GameObject/" + menu_name)]
        public static void ShowHiddenMenuObjects()
        {
            bool isHidden = HiddenGameObject.IsObjectsHidden;
            Menu.SetChecked(menu_name, isHidden);
            HiddenGameObject.IsObjectsHidden = !isHidden;

            foreach (HiddenGameObject hiddenGameObject in GameObject.FindObjectsOfType<HiddenGameObject>())
            {
                hiddenGameObject.RefreshHiddenState();
            }
        }
    }
}
#endif
