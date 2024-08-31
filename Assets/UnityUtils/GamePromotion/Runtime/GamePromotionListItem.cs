using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.GamePromotion
{
    public class GamePromotionListItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button downloadButton;

        private OtherGame otherGame;
        public OtherGame OtherGame
        {
            get { return otherGame; }
            set
            {
                otherGame = value;
                image.sprite = otherGame.image;
            }
        }
        
        void Start()
        {
            downloadButton.onClick.AddListener(() =>
            {
#if UNITY_IPHONE
            Application.OpenURL(otherGame.iosStoreUrl);
#else
                Application.OpenURL(otherGame.androidStoreUrl);
#endif
            });
        }

    }
}

