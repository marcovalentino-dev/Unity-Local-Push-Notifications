using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GladioGames.Android;
using GladioGames.iOS;

namespace GladioGames
{
    public static class LocalNotificationEventsManager
    {
        #region Authorization Request Completed
        public delegate void OnAuthorizationRequestCompleted(bool accepted);
        public static event OnAuthorizationRequestCompleted OnAuthorizationRequestCompletedEvent;
        public static void AuthorizationRequestCompleted(bool accepted)
        {
            OnAuthorizationRequestCompletedEvent?.Invoke(accepted);
        }
        #endregion
    }


    public class LocalNotificationManager
    {
        #region Android
        //Schedule a notification
        public static void AndroidScheduleNotification(string title, string text, DateTime date, bool cancelPrevious = true, string smallIconID = null, string largeIconID = null, TimeSpan? fireInterval = null)
        {
            AndroidLocalNotificationSupport.AndroidScheduleNotification(title, text, date, cancelPrevious, smallIconID, largeIconID, fireInterval);
        }
        //Check if last notification arrived
        public static bool AndroidLastNotificationReceived()
        {
            return AndroidLocalNotificationSupport.AndroidLastNotificationReceived();
        }
        //Check if the app was opened by a notification
        public static bool AndroidIsApplicationOpenedByNotification()
        {
            return AndroidLocalNotificationSupport.AndroidIsApplicationOpenedByNotification();
        }
        #endregion

        #region iOS
        //Request user permission
        public static IEnumerator iOSRequestAuthorization()
        {
            return IOSLocalNotificationSupport.iOSRequestAuthorization();
        }
        //Check authorization status
        public static bool iOSUserAlreadyAuthorized()
        {
            return IOSLocalNotificationSupport.iOSUserAlreadyAuthorized();
        }
        //Schedule a notification
        public static void iOSScheduleNotification(string title, string subtitle, string text, DateTime date, bool repeat = true, bool cancelPrevious = false)
        {
            IOSLocalNotificationSupport.iOSScheduleNotification(title, subtitle, text, date, repeat, cancelPrevious);
        }

        //Check if the app was opened by a notification
        public static bool iOSIsApplicationOpenedByNotification()
        {
            return IOSLocalNotificationSupport.iOSIsApplicationOpenedByNotification();
        }
        #endregion
    }
}