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
                Debug.LogWarning("Ubicación no obtenida: " + error);
                // Coordenadas de Barcelona (aproximadamente)
                float barcelonaLat = 41.38879f;
                float barcelonaLon = 2.15899f;

                Debug.Log("Usando ubicación por defecto: Barcelona");
                StartCoroutine(weatherService.GetTomorrowWeather(
                    barcelonaLat, barcelonaLon,
                    (resumen) => Debug.Log(resumen + " (Barcelona)"),
                    (climaError) => Debug.LogError("Clima error con ubicación por defecto: " + climaError)
                ));
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
            Debug.LogWarning("Permiso de ubicación denegado por el usuario. Usando ubicación por defecto.");
            // Coordenadas de Barcelona directamente si no hay permiso
            float barcelonaLat = 41.38879f;
            float barcelonaLon = 2.15899f;

            StartCoroutine(weatherService.GetTomorrowWeather(
                barcelonaLat, barcelonaLon,
                (resumen) => Debug.Log(resumen + " (Barcelona)"),
                (error) => Debug.LogError("Clima error con ubicación por defecto: " + error)
            ));
        }
#else
        StartCoroutine(GetWeatherAtCurrentLocation());
#endif
    }
}
