🛎
# Local Push Notification Manager
#
#
## Description
Handle local push notifications for mobile devices.

## Import
```sh
🚨 Unity Mobile Notification Package needed → If you don’t have add it
```
- Add the Local Push Notifications Folder to your project in Plugins folder

## Usage
> Android


##### Send a notification
#

```sh
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
```

##### Send a notification with custom icons
#

1. Set Icons 
    - Prepare the icon (SMALL) (If not configured, the icon app will be used)
![alt text](https://i.imgur.com/eihhyYY.png "Logo Title Text 1")

