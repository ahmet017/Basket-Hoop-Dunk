using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityUtils.GamePromotion
{
    [RequireComponent(typeof(Button))]
    public class GamePromotionButton : MonoBehaviour
    {
        private Button button;
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                GamePromotionManager.Show();
            });
        }
    }
}

