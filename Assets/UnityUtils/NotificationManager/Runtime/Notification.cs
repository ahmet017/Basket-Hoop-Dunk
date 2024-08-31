using UnityEngine;
using UnityEngine.Localization;

namespace UnityUtils.NotificationManager
{
    [System.Serializable]
    public class Notification
    {
        public NotificationFormat format;
        public LocalizedString titleLocalized;
        public LocalizedString messageLocalized;
        public string titleString;
        public string messageString;
        public int duration;
    }
}