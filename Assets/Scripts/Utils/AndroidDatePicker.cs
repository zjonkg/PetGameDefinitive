using UnityEngine;
using System;
using TMPro; // 👈 Importante para usar TextMeshPro
using static UnityEngine.EventSystems.EventTrigger;

public class AndroidDatePicker : MonoBehaviour
{
    public TMP_Text dateText; // 👈 Asigna un TextMeshPro Text aquí desde el inspector

    public void ShowDatePicker()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // Pasar la referencia del TMP_Text
            DateSetListener.dateText = dateText;

            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                using (AndroidJavaObject datePickerDialog = new AndroidJavaObject(
                    "android.app.DatePickerDialog",
                    activity,
                    new DateSetListener(),
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day))
                {
                    datePickerDialog.Call("show");
                }
            }));
        }
#endif
    }

    private class DateSetListener : AndroidJavaProxy
    {
        public static TMP_Text dateText;

        public DateSetListener() : base("android.app.DatePickerDialog$OnDateSetListener") { }

        void onDateSet(AndroidJavaObject view, int year, int month, int dayOfMonth)
        {
            string formattedDate = $"{dayOfMonth:D2}/{(month + 1):D2}/{year}";
            Debug.Log("Fecha seleccionada: " + formattedDate);

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if (dateText != null)
                {
                    dateText.text = formattedDate;
                    UIController.Instance.ValidateRegisterFields();
                }
            });
        }
    }
}