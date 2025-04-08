using MyGame.Auth;
using TMPro;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    private string apiUrl = "https://api-management-pet-production.up.railway.app/user/login";

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
                textMeshPro.text = "Login exitoso. Token: " + response.message; // Asumiendo que el token esté en 'response.token'


                // Aquí puedes guardar el token o redirigir de escena
            },
            (error) =>
            {
                Debug.LogError("Error en login: " + error);
            }
        ));
    }
}
