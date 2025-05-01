using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class WeatherTester : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private LocationServiceHandler locationService;

    void Start()
    {
        StartCoroutine(CheckPermissionAndRun());
    }

    private IEnumerator GetWeatherAtCurrentLocation()
    {
        yield return locationService.GetDeviceLocation(
            (coords) =>
            {
                StartCoroutine(weatherService.GetTomorrowWeather(
                    coords.x, coords.y,
                    (resumen) => Debug.Log(resumen),
                    (error) => Debug.LogError("Clima error: " + error)
                ));
            },
            (error) =>
            {
                Debug.LogError("Ubicación error: " + error);
            }
        );
    }

    IEnumerator CheckPermissionAndRun()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(2f); // Espera a que el usuario responda
        }

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            StartCoroutine(GetWeatherAtCurrentLocation());
        }
        else
        {
            Debug.LogError("Permiso de ubicación denegado por el usuario.");
        }
#else
        StartCoroutine(GetWeatherAtCurrentLocation());
#endif


    }
}