using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace UnityUtils.NotificationManager
{
    public class NotificationManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            DontDestroyOnLoad(Instantiate(Resources.Load("NotificationManager")));
        }

        public static readonly string NOTIFICATION_CHANNEL_ID = "notifications";
        public static readonly string NOTIFICATION_REQUEST_KEY = "notificationsRequested";
        public static bool IsAppOpenedWithNotification;

        private NotificationSettings settings;
        private int testingDuration = 10;

        IEnumerator Start()
        {
            settings = Resources.Load<NotificationSettings>("NotificationSettings");
            if (!settings.notificationsEnabled)
            {
                Destroy(gameObject);
                yield break;
            }


#if UNITY_ANDROID
            if (AndroidNotificationCenter.GetLastNotificationIntent() != null)
            {
                IsAppOpenedWithNotification = true;
            }

            AndroidNotificationCenter.CancelAllNotifications();

            var channel = new AndroidNotificationChannel()
            {
                Id = NOTIFICATION_CHANNEL_ID,
                Name = "Notifications",
                Importance = Importance.Default,
                Description = "Reminder",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            if (PlayerPrefs.GetInt(NOTIFICATION_REQUEST_KEY, 0) != 1)
                StartCoroutine(RequestNotificationPermission());
#elif UNITY_IOS
            if (iOSNotificationCenter.GetLastRespondedNotification() != null)
            {
                IsAppOpenedWithNotification = true;
            }

            iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif

            foreach (Notification notification in settings.notifications)
            {
                if (notification.format == NotificationFormat.String)
                {
                    ScheduleNotification(notification.titleString, notification.messageString, notification.duration);
                }
                else
                {
                    var titleTask = notification.titleLocalized.GetLocalizedStringAsync();
                    var messageTask = notification.messageLocalized.GetLocalizedStringAsync();
                    yield return new WaitUntil(() => titleTask.IsDone && messageTask.IsDone);
                    ScheduleNotification(titleTask.Result, messageTask.Result, notification.duration);
                }
            }
        }

        public void ScheduleNotification(string title, string message, int duration)
        {
#if UNITY_ANDROID
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = message;
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        if (settings.notificationTestingEnabled)
            notification.FireTime = System.DateTime.Now.AddSeconds(testingDuration);
        else
            notification.FireTime = System.DateTime.Now.AddHours(duration);

        AndroidNotificationCenter.SendNotification(notification, NOTIFICATION_CHANNEL_ID);

#elif UNITY_IOS
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = settings.notificationTestingEnabled ? new TimeSpan(0, 0, testingDuration) : new TimeSpan(duration, 0, 0),
            Repeats = false
        };
        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = NOTIFICATION_CHANNEL_ID,
            Title = title,
            Body = message,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };
        iOSNotificationCenter.ScheduleNotification(notification);
#endif

        }

        IEnumerator RequestNotificationPermission()
        {
#if UNITY_ANDROID
            var request = new PermissionRequest();
            float elapsedTime = 0f;
            float timeOutDuration = 30f;

            while (request.Status == PermissionStatus.RequestPending)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= timeOutDuration)
                {
                    Debug.Log("NOTIFICATION PERMISSION REQUEST HAS BEEN TIMED OUT.");
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
            // here use request.Status to determine users response

            Debug.Log("REQUEST STATUS : " + request.Status);
            PlayerPrefs.SetInt(NOTIFICATION_REQUEST_KEY, 1);
            PlayerPrefs.Save();
            yield break;
#else
            yield break;
#endif
        }

    }
}

