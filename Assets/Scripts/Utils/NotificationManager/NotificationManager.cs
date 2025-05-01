using Unity.Notifications.Android;
using System;
using UnityEngine;

public static class NotificationManager
{
    private const string ChannelId = "weather_channel";

    public static void Initialize()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = ChannelId,
            Name = "Weather Notifications",
            Importance = Importance.Default,
            Description = "Notificaciones del clima diario",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public static void ScheduleNotification(string title, string message, DateTime time)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = message,
            FireTime = time
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
}
