using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.URLManager
{
    [RequireComponent(typeof(Button))]
    public class PrivacyPolicyButton : MonoBehaviour
    {
        private Button button;
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => Application.OpenURL(URLManager.PrivacyPolicyURL));
        }
    }
}


