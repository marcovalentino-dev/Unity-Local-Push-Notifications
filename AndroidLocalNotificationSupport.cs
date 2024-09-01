using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

namespace GladioGames.Android
{

    public class AndroidLocalNotificationSupport
    {
        public static string ANDROID_CHANNEL_ID = "gladio_games";
        public static string NOTIFICATION_ID_KEY = "gladio_games_local_notification";

        public static void AndroidScheduleNotification(string title, string text, DateTime date, bool cancelPrevious = true, string smallIconID = null, string largeIconID = null, TimeSpan ? fireInterval = null)
        {
#if UNITY_ANDROID
            //Create a custom notification channel id key for the game
            if (ANDROID_CHANNEL_ID == "gladio_games")
            {
                ANDROID_CHANNEL_ID = $"gladio_games_{Application.productName}";
               // Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: ANDROID_CHANNEL_ID: {ANDROID_CHANNEL_ID}");
            }

            string storedChannelId = PlayerPrefs.GetString(ANDROID_CHANNEL_ID, "");
            //If the notification channel does not exist yet, we need to create it
            if (storedChannelId == "")
            {
                //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: STORE CHANNEL ID NULL");
                var channel = new AndroidNotificationChannel()
                {
                    Id = ANDROID_CHANNEL_ID,
                    Name = "Reminder Channel",
                    Importance = Importance.Default,
                    Description = "Remind to open the app notifications",
                };

                PlayerPrefs.SetString(ANDROID_CHANNEL_ID, channel.Id);
                storedChannelId = channel.Id;


                AndroidNotificationCenter.RegisterNotificationChannel(channel);
            }

            //Will check if we need to send a new notification or update an existing one in case of cancelPrevious
            bool sendNew = true;

            //Create the notification
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = text;
            notification.FireTime = date;
            if (smallIconID != null)
                notification.SmallIcon = smallIconID;
            if (largeIconID != null)
                notification.LargeIcon = largeIconID;
            if (fireInterval != null)
            {
                notification.RepeatInterval = fireInterval;
            }
            if (cancelPrevious)
            {
                //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: CANCEL PREVIOUS");
                //Try to retrieve previous notification id
                int previousNotificationId = PlayerPrefs.GetInt(NOTIFICATION_ID_KEY, -999);
                //If a notification was scheduled and its notification ID was stored locally
                if (previousNotificationId != -999)
                {
                    //Get notification status
                    var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(previousNotificationId);


                    if (notificationStatus == NotificationStatus.Scheduled)
                    {
                        //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: PREVIOUS WAS SCHEDULE");
                        // Replace the scheduled notification with a new notification.
                        AndroidNotificationCenter.UpdateScheduledNotification(previousNotificationId, notification, ANDROID_CHANNEL_ID);
                        sendNew = false;
                    }
                    else if (notificationStatus == NotificationStatus.Delivered)
                    {
                       // Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: PREVIOUS WAS DELIVERED");
                        // Remove the previously shown notification from the status bar or schedule.
                        AndroidNotificationCenter.CancelAllNotifications();
                    }
                    else if (notificationStatus == NotificationStatus.Unknown)
                    {
                        //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: PREVIOUS WAS UNKNOWN");
                        // Remove the previously shown notification from the status bar or schedule.
                        AndroidNotificationCenter.CancelAllNotifications();
                    }
                } else
                {
                    //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: NO PREVIOUS FOUND");
                    AndroidNotificationCenter.CancelAllNotifications();
                }
            }


            if (sendNew)
            {
                //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: SEND NEW TO: {storedChannelId}");
                //Schedule notification
                int notificationID = AndroidNotificationCenter.SendNotification(notification, storedChannelId);
                //Save its id
                //Debug.Log($"!!!!!!!!!!!!!!!!GLADIO_NOTIFICATIONS: NOTIFICATIONID: {notificationID}");
                PlayerPrefs.SetInt(NOTIFICATION_ID_KEY, notificationID);
            }
#endif
        }


        public static bool AndroidLastNotificationReceived()
        {

            bool received = false;
#if UNITY_ANDROID
            //Try to retrieve previous notification id
            int previousNotificationId = PlayerPrefs.GetInt(NOTIFICATION_ID_KEY, -999);
            //If a notification was scheduled and its notification ID was stored locally
            if (previousNotificationId != -999)
            {
                //Get notification status
                var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(previousNotificationId);

                if (notificationStatus == NotificationStatus.Delivered)
                {
                    received = true;
                }
            }
#endif
            return received;

        }


        public static bool AndroidIsApplicationOpenedByNotification()
        {

            bool notificationIntent = false;
#if UNITY_ANDROID
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                var id = notificationIntentData.Id;
                var channel = notificationIntentData.Channel;
                var notification = notificationIntentData.Notification;
                notificationIntent = true;
            }
#endif
            return notificationIntent;

        }
    }
}