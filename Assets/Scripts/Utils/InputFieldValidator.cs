using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldValidator : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField passwordConfirmInput;
    public TMP_InputField usernameInput;

    [Header("Validation Indicators")]
    public Image emailValidImage;
    public Image emailInvalidImage;
    public Image passwordValidImage;
    public Image passwordInvalidImage;
    public Image usernameValidImage;
    public Image usernameInvalidImage;
    public Image password2ValidImage;
    public Image password2InvalidImage; 

    // Validación de correo electrónico
    public void OnEmailEndEdit(string text)
    {
        bool isValid = InputValidator.IsValidEmail(text);
        ShowValidation(emailValidImage, emailInvalidImage, isValid);
        Debug.Log(isValid ? "✔️ Email válido" : "❌ Email inválido");
    }

    public void OnPasswordEndEdit(string text)
    {
        bool isValid = InputValidator.IsValidPassword(text);
        ShowValidation(passwordValidImage, passwordInvalidImage, isValid);
        Debug.Log(isValid ? "✔️ Contraseña válida" : "❌ Contraseña inválida");
    }

    public void OnPasswordConfirmEndEdit(string confirmText)
    {
        string originalPassword = passwordInput.text;
        bool isMatch = confirmText == originalPassword;
        ShowValidation(password2ValidImage, password2InvalidImage, isMatch);
        Debug.Log(isMatch ? "✔️ Contraseñas coinciden" : "❌ Contraseñas no coinciden");
    }

    public void OnUsernameEndEdit(string text)
    {
        bool isValid = InputValidator.IsValidUsername(text);
        ShowValidation(usernameValidImage, usernameInvalidImage, isValid);
        Debug.Log(isValid ? "✔️ Nombre de usuario válido" : "❌ Nombre de usuario inválido");
    }

    void Start()
    {
        emailInput.onEndEdit.AddListener(OnEmailEndEdit);
        passwordInput.onEndEdit.AddListener(OnPasswordEndEdit);
        passwordConfirmInput.onEndEdit.AddListener(OnPasswordConfirmEndEdit); // Nuevo listener
        usernameInput.onEndEdit.AddListener(OnUsernameEndEdit);
    }

    private void ShowValidation(Image valid, Image invalid, bool isValid)
    {
        if (valid != null) valid.gameObject.SetActive(isValid);
        if (invalid != null) invalid.gameObject.SetActive(!isValid);
    }
}
