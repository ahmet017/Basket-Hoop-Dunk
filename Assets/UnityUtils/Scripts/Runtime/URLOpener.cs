using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityUtils
{
    [RequireComponent(typeof(Button))]
    public class URLOpener : MonoBehaviour
    {
        public string url;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(url)) return;
                Application.OpenURL(url);
            });
        }
    }
}

