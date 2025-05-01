using UnityEngine;

using System.Collections;
using System;

public class LocationServiceHandler : MonoBehaviour
{
    public IEnumerator GetDeviceLocation(Action<Vector2> onSuccess, Action<string> onError)
    {
        if (!Input.location.isEnabledByUser)
        {
            onError?.Invoke("Ubicación no habilitada por el usuario.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 15;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            onError?.Invoke("No se pudo determinar la ubicación.");
        }
        else
        {
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            onSuccess?.Invoke(new Vector2(latitude, longitude));
        }

        Input.location.Stop();
    }
}
