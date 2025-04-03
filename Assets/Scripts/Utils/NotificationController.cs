using System;
using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationController : MonoBehaviour
{
    private const string ChannelId = "FUA_TIO";
    private const string GroupId = "Main";

    private void Start()
    {
        StartCoroutine(AllowNotification());
    }

    public void SendNotification(string title, string message, int delaySeconds = 5)
    {
#if UNITY_ANDROID
        DateTime fireTime = DateTime.Now.AddSeconds(delaySeconds);
        CreateNotification(title, message, fireTime);
#endif
    }

    private void CreateNotification(string title, string message, DateTime dateTime)
    {
#if UNITY_ANDROID
        // Registrar grupo de notificación
        var group = new AndroidNotificationChannelGroup
        {
            Id = GroupId,
            Name = "Main notifications"
        };
        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

        // Registrar canal de notificación
        var channel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Canal de notificaciones",
            Group = GroupId
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // Crear y enviar la notificación
        var notification = new AndroidNotification
        {
            Title = title,
            Text = message,
            FireTime = dateTime
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelId);
#endif
    }

    private IEnumerator AllowNotification()
    {
#if UNITY_ANDROID
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
        {
            yield return null;
        }
#endif
    }
}
