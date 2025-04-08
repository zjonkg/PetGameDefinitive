using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public void OnLoginButtonPressed()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        AuthManager.Instance.Login(email, password);
    }
}
