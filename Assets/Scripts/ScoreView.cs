using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreView : MonoBehaviour
{
    private TextMeshProUGUI textView;

    void Start()
    {
        textView = GetComponent<TextMeshProUGUI>();
        textView.text = GameManager.BestScore.ToString();
    }

}
