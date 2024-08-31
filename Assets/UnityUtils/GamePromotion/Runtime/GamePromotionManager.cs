using UnityEngine;


namespace UnityUtils.GamePromotion
{
    public static class GamePromotionManager
    {
        public static void Show()
        {
            GamePromotionSettings settings = Resources.Load<GamePromotionSettings>("GamePromotionSettings");
            if (settings == null || settings.otherGames == null || settings.otherGames.Count == 0) return;

            Object.Instantiate(Resources.Load<GamePromotionCanvas>("GamePromotionCanvas"));
        }
    }

}




