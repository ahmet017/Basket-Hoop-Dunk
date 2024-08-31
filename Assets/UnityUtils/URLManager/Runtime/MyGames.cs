using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGames : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/developer?id=aaa+games&hl=en_US"));
    }
}
