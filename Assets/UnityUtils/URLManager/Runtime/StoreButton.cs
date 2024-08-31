using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.URLManager
{
    [RequireComponent(typeof(Button))]
    public class StoreButton : MonoBehaviour
    {
        private Button button;
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => Application.OpenURL(URLManager.StoreURL));
        }
    }
}

