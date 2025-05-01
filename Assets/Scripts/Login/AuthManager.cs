using MyGame.Auth;
using Newtonsoft.Json;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    private string apiUrl = "https://api-management-pet-production.up.railway.app/user/login";
    private string registerUrl = "https://api-management-pet-production.up.railway.app/user/signup";

    [SerializeField]
    public TMP_Text textMeshPro; // Cambia el tipo a TMP_Text


    private void Awake()
    {
        // Si no hay una instancia previa, esta será la única
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener al cambiar de escena (opcional)
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados si ya hay una instancia
        }
    }

    public void Login(string email, string password)
    {
        LoginRequest requestData = new LoginRequest(email, password);

        StartCoroutine(HttpService.Instance.SendRequest<LoginResponse>(
            apiUrl,
            "POST",
            requestData,
            (response) =>
            {
                Debug.Log("Login exitoso. Token: " + response);
                PlayerPrefs.SetString("player_id", response.id.ToString());
                PlayerPrefs.Save();


                SceneManager.LoadScene("House");


            },
            (error) =>
            {
                Debug.LogError("Error en login: " + error);
            }
        ));
    }

    public void Register(string username, string email, string password, string birthday)
    {
        DateTime parsedDate;
        if (!DateTime.TryParse(birthday, out parsedDate))
        {
            Debug.LogError("La fecha de nacimiento no tiene un formato válido.");
            return;
        }

        string formattedBirthday = parsedDate.ToString("yyyy/MM/dd");

        RegisterRequest requestData = new RegisterRequest(username, email, password, formattedBirthday, "");

        // Imprimir los datos enviados en la solicitud
        Debug.Log("Datos enviados en la solicitud:");
        Debug.Log(JsonConvert.SerializeObject(requestData, Formatting.Indented));

        StartCoroutine(HttpService.Instance.SendRequest<RegisterResponse>(
            registerUrl,
            "POST",
            requestData,
            (response) =>
            {
                Debug.Log("Registro exitoso. Respuesta del servidor:");
                Debug.Log(JsonConvert.SerializeObject(response, Formatting.Indented));
                textMeshPro.text = "Registro exitoso. Mensaje: " + response.message;
            },
            (error) =>
            {
                Debug.LogError("Error en el registro. Respuesta del servidor:");
                Debug.LogError(error);

                try
                {
                    // Deserializa el error como HTTPValidationError
                    var validationError = JsonConvert.DeserializeObject<HTTPValidationError>(error);

                    // Itera sobre los detalles del error
                    foreach (var detail in validationError.detail)
                    {
                        Debug.LogError($"Campo: {string.Join(", ", detail.loc)}, Mensaje: {detail.msg}, Tipo: {detail.type}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("No se pudo analizar el error JSON: " + ex.Message);
                }
            }
        ));
    }
}

