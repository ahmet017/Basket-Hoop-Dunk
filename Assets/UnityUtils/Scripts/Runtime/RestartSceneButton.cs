using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityUtils
{
    [RequireComponent(typeof(Button))]
    public class RestartSceneButton : MonoBehaviour
    {
        private Button button;

        void Start()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
    }
}

