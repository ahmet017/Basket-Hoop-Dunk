using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIClick : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => AudioManager.Instance.Click());
    }
}
