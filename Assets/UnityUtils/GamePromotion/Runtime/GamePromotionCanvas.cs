using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UnityUtils.GamePromotion
{
    public class GamePromotionCanvas : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GamePromotionSettings settings;
        [SerializeField] private RectTransform container;
        [SerializeField] private GamePromotionListItem listItemPrefab;

        void Start()
        {
            for (int i = settings.otherGames.Count - 1; i >= 0; i--)
            {
                if (settings.otherGames[i] == null || settings.otherGames[i].image == null) 
                    return;

                if ((IsIOS() && settings.otherGames[i].iosStoreUrl == null) || (!IsIOS() && settings.otherGames[i].androidStoreUrl == null))
                    return;

                GamePromotionListItem item = Instantiate(listItemPrefab, container.transform);
                item.OtherGame = settings.otherGames[i];
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);

            closeButton.onClick.AddListener(() => Destroy(gameObject));
        }

        private bool IsIOS()
        {
#if UNITY_IPHONE
            return true;
#else
            return false;
#endif
        }
    }
}

