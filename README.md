🛎
Local Push Notification Manager

Description
Handle local push notifications for mobile devices.

Import
🚨 Unity Mobile Notification Package needed → If you don’t have add it
  
  Add the Local Push Notifications Folder to your project in Plugin Folder

Usage
Android
Send a notification
//DateTime -> when Tomorrow at 10:00:00 in the example
//Time managament is an internal Gladio Games plugin of the Starter Kit
DateTime tomorrow = TimeManagement.TomorrowAtTime(10,0,0);
//Notification title
string title = "Title";
//Notification text
string text = "Text";
//Cancel or update previous notification (default true)
bool cancelPrevious = true;
//Send a notification
LocalNotificationManager.AndroidScheduleNotification(title, text, tomorrow, cancelPrevious);



