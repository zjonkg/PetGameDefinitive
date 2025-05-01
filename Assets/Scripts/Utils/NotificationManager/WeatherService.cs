using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class WeatherForecastResponse
{
    public Location location;
    public Forecast forecast;

    [Serializable]
    public class Location
    {
        public string name;
        public string country;
        public string localtime;
    }

    [Serializable]
    public class Forecast
    {
        public ForecastDay[] forecastday;
    }

    [Serializable]
    public class ForecastDay
    {
        public string date;
        public Day day;
        public Astro astro;
        public Hour[] hour;
    }

    [Serializable]
    public class Day
    {
        public float maxtemp_c;
        public float mintemp_c;
        public float avgtemp_c;
        public float maxwind_kph;
        public float totalprecip_mm;
        public float avghumidity;
        public Condition condition;
    }

    [Serializable]
    public class Astro
    {
        public string sunrise;
        public string sunset;
    }

    [Serializable]
    public class Hour
    {
        public string time;
        public float temp_c;
        public float wind_kph;
        public float humidity;
        public Condition condition;
    }

    [Serializable]
    public class Condition
    {
        public string text;
        public string icon;
    }
}

public class WeatherService : MonoBehaviour
{
    [Header("Weather API Settings")]
    [SerializeField] private string apiKey = "52d69fb145c14ad0bad140039251503";
    [SerializeField] private float latitude = 41.4154752f;
    [SerializeField] private float longitude = 2.195456f;

    private const string BASE_URL = "https://api.weatherapi.com/v1/forecast.json";


    public IEnumerator GetTomorrowWeather(float latitude, float longitude, Action<string> onSummary, Action<string> onError)
    {
        string query = string.Format(CultureInfo.InvariantCulture, "{0},{1}", latitude, longitude);
        string url = $"{BASE_URL}?key={apiKey}&q={query}&lang=es&days=2";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(request.error);
        }
        else
        {
            try
            {
                WeatherForecastResponse data = JsonUtility.FromJson<WeatherForecastResponse>(request.downloadHandler.text);

                var tomorrow = data.forecast.forecastday[1];
                var day = tomorrow.day;

                string resumen = $"📍 {data.location.name}, {data.location.country}\n" +
                                 $"📅 Fecha: {tomorrow.date}\n" +
                                 $"🌤️ Clima: {day.condition.text}\n" +
                                 $"🌡️ Máx: {day.maxtemp_c}°C / Mín: {day.mintemp_c}°C\n" +
                                 $"💧 Humedad: {day.avghumidity}%\n" +
                                 $"🌬️ Viento: {day.maxwind_kph} km/h\n" +
                                 $"🌄 Amanecer: {tomorrow.astro.sunrise} / 🌇 Atardecer: {tomorrow.astro.sunset}\n\n" +
                                 $"🕒 Clima por hora:\n";

                foreach (var hour in tomorrow.hour)
                {
                    string hourTime = hour.time.Substring(11);
                    resumen += $" - {hourTime}: {hour.temp_c}°C, {hour.condition.text}, 💧{hour.humidity}%\n";
                }

                onSummary?.Invoke(resumen);
            }
            catch (Exception e)
            {
                onError?.Invoke($"Error parseando JSON: {e.Message}");
            }
        }
    }

}
