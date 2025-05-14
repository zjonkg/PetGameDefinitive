using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using UnityEngine;

public class NotificationManagers : MonoBehaviour
{
    private FirebaseApp app;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Inicializar la instancia de Firebase
                app = Firebase.FirebaseApp.DefaultInstance;

                // Ahora sí puedes suscribirte a los eventos
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;

                // Opcional: solicitar permiso para notificaciones
                FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(requestTask => {
                    Debug.Log("Permiso para notificaciones solicitado.");
                });

                Debug.Log("Firebase inicializado correctamente.");
            }
            else
            {
                Debug.LogError($"No se pudieron resolver las dependencias de Firebase: {dependencyStatus}");
            }
        });
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Token FCM recibido: " + token.Token);
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Mensaje FCM recibido desde: " + e.Message.From);
        Debug.Log("Contenido: " + e.Message.Notification?.Body ?? "Sin cuerpo");
    }
}