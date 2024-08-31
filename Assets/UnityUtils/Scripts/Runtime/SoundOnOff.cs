using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityUtils
{
    [RequireComponent(typeof(Button))]
    public class SoundOnOff : MonoBehaviour
    {
        [SerializeField] private Sprite soundOnSprite, soundOffSprite;
        [SerializeField] private Image image;
        private Button button;


        void Start()
        {
            button = GetComponent<Button>();
            SetSoundOff(IsSoundOff());

            button.onClick.AddListener(() =>
            {
                SetSoundOff(!IsSoundOff());
            });

        }

        public static bool IsSoundOff()
        {
            return PlayerPrefs.GetInt("soundOff", 0) == 1;
        }
        private void SetSoundOff(bool isOff)
        {
            PlayerPrefs.SetInt("soundOff", isOff ? 1 : 0);
            PlayerPrefs.Save();

            AudioListener.volume = isOff ? 0 : 1;
            image.sprite = isOff ? soundOffSprite : soundOnSprite;
        }
    }

}
