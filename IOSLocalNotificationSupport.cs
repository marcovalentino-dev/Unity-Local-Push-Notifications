using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;

namespace GladioGames.iOS
{
    public class IOSLocalNotificationSupport
    {
        public static string iOS_DEVICE_TOKEN = "ios_device_token";
        public static string iOS_NOTIFICATION_ID = "ios_notification_id";

        private static string iOSNotificationKey
        {
            get { return PlayerPrefs.GetString(iOS_NOTIFICATION_ID, ""); }
            set { PlayerPrefs.GetString(iOS_NOTIFICATION_ID, value); }
        }

        private static string DeviceToken
        {
            get { return PlayerPrefs.GetString(iOS_DEVICE_TOKEN, ""); }
            set { PlayerPrefs.GetString(iOS_DEVICE_TOKEN, value); }
        }

        //Request Authorization Coroutine
        public static IEnumerator iOSRequestAuthorization()
        {
#if UNITY_IOS
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using (var req = new AuthorizationRequest(authorizationOption, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(res);

                DeviceToken = req.DeviceToken;

                LocalNotificationEventsManager.AuthorizationRequestCompleted(req.Granted);
            }
#endif

#if !UNITY_IOS
            yield return null;
#endif
        }

        //Check Permission Status
        public static bool iOSUserAlreadyAuthorized()
        {
            bool authorized = false;
#if UNITY_IOS
            authorized = iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Authorized;

#endif
            return authorized;
        }

        //Schedule an iOS notificaition
        public static void iOSScheduleNotification(string title, string subtitle, string text, DateTime date, bool repeat = true, bool cancelPrevious = false)
        {
#if UNITY_IOS
            if (cancelPrevious && iOSNotificationKey != "")
            {
                iOSNotificationCenter.RemoveScheduledNotification(iOSNotificationKey);
                iOSNotificationCenter.RemoveDeliveredNotification(iOSNotificationKey);
            }


            var calendarTrigger = new iOSNotificationCalendarTrigger()
            {
                Year = date.Year,
                Month = date.Month,
                Day = date.Day,
                Hour = date.Hour,
                Minute = date.Minute,
                Second = date.Second,
                Repeats = repeat
            };

            if (repeat)
            {
                calendarTrigger = new iOSNotificationCalendarTrigger()
                {
                    Hour = date.Hour,
                    Minute = date.Minute,
                    Repeats = repeat
                };
            }

            //var repeatingCalendarTrigger = new iOSNotificationCalendarTrigger()
            //{
            //    //Year = 2020,
            //    //Month = 6,
            //    //Day = 1,
            //    Hour = date.Hour,
            //    Minute = date.Minute,
            //    // Second = 0
            //    Repeats = true
            //};

            //var calendarTrigger = repeat ? repeatingCalendarTrigger : specificCalendarTrigger;


            var notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = "_notification_01",
                Title = title,
                Body = text,
                Subtitle = subtitle,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "daily_gift",
                ThreadIdentifier = "thread1",
                Trigger = calendarTrigger,
            };

            iOSNotificationKey = notification.Identifier;
            iOSNotificationCenter.ScheduleNotification(notification);
#endif
        }

        //Check if the app was opened by a notification
        public static bool iOSIsApplicationOpenedByNotification()
        {
            bool wasOpenByNotification = false;
#if UNITY_IOS
            wasOpenByNotification = iOSNotificationCenter.GetLastRespondedNotification() != null;
#endif
            return wasOpenByNotification;
        }
    }
}
