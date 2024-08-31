using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


namespace UnityUtils.NotificationManager
{
    public class NotificationSettings : ScriptableObject
    {
        [HideInInspector] public List<Notification> notifications = new List<Notification>();
        [HideInInspector] public bool notificationTestingEnabled;
        [HideInInspector] public bool notificationsEnabled;
    }
}


