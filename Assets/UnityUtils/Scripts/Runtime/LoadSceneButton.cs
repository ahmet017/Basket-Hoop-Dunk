using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityUtils
{
    [RequireComponent(typeof(Button))]
    public class LoadSceneButton : MonoBehaviour
    {
        private Button button;
        [HideInInspector] public string scenePath;

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(scenePath);
            });
        }
    }
}


