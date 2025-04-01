using System;
using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationController : MonoBehaviour
{
    private const string ChannelId = "FUA TIO";
    private const string GroupId = "Main";

    private void Start()
    {
        StartCoroutine(AllowNotification());
    }

    public void ActivateNotification()
    {
#if UNITY_ANDROID
        DateTime activateTime = DateTime.Now.AddSeconds(5);
        CreateNotification(activateTime);
#endif
    }

    private void CreateNotification(DateTime dateTime)
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
            Description = "No se jaja",
            Group = GroupId
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

       
        var notification = new AndroidNotification
        {
            Title = "OJITO",
            Text = "Hola Laura uwu",
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
