using MyGame.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
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
                PlayerPrefs.SetString("has_mascot", response.has_mascot.ToString());
                PlayerPrefs.Save();

                if (response.has_mascot)
                {
                    SceneManager.LoadScene("House");
                    return;
                }


                SceneManager.LoadScene("QRScreen");

            },
            (error) =>
            {
                AnimationError.Instance.ShowPopup("Error", "Credenciales incorrectas");
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


        Debug.Log("Datos enviados en la solicitud:");
        Debug.Log(JsonConvert.SerializeObject(requestData, Formatting.Indented));

        StartCoroutine(HttpService.Instance.SendRequest<RegisterResponse>(
            registerUrl,
            "POST",
            requestData,
            (response) =>
            {
                Debug.Log("Registro exitoso. Respuesta del servidor:");
                PopupMessage.Instance.ShowPopup("Exitoso", "Cuenta creada exitosamente");
                StartCoroutine(OnRegistrationSuccess(email, password));
            },
            (error) =>
            {
                AnimationError.Instance.ShowPopup("Error", "Cuenta no creada, correo electrónico o usuario ya registrado");
            }
        ));
    }

    private IEnumerator OnRegistrationSuccess(string email, string password)
    {
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoginCoroutine(email, password));
    }
    private IEnumerator LoginCoroutine(string email, string password)
    {
        LoginRequest requestData = new LoginRequest(email, password);

        yield return StartCoroutine(HttpService.Instance.SendRequest<LoginResponse>(
            apiUrl,
            "POST",
            requestData,
            (response) =>
            {
                Debug.Log("Login exitoso. Token: " + response);
                PlayerPrefs.SetString("player_id", response.id.ToString());
                PlayerPrefs.SetString("has_mascot", response.has_mascot.ToString());
                PlayerPrefs.Save();

                if (response.has_mascot)
                {
                    SceneManager.LoadScene("House");
                }
                else
                {
                    SceneManager.LoadScene("QRScreen");
                }
            },
            (error) =>
            {
                AnimationError.Instance.ShowPopup("Error", "Credenciales incorrectas");
                Debug.LogError("Error en login: " + error);
            }
        ));
    }

}