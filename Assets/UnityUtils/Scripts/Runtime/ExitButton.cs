using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityUtils
{
    [RequireComponent(typeof(Button))]
    public class ExitButton : MonoBehaviour
    {
        private Button button;

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

    }
}

