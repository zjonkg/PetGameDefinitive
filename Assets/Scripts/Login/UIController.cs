using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Login Fields")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    [Header("Register Fields")]
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;
    public TMP_Text registerDateOfBirthInput;
    public Button registerButton;

    [Header("UI Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    public static UIController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Agregar listeners a cada input para que se valide en tiempo real
        emailInput.onValueChanged.AddListener(delegate { ValidateLoginFields(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidateLoginFields(); });

        registerEmailInput.onValueChanged.AddListener(delegate { ValidateRegisterFields(); });
        registerUsernameInput.onValueChanged.AddListener(delegate { ValidateRegisterFields(); });
        registerPasswordInput.onValueChanged.AddListener(delegate { ValidateRegisterFields(); });
        registerConfirmPasswordInput.onValueChanged.AddListener(delegate { ValidateRegisterFields(); });

        // Validación inicial
        ValidateLoginFields();
        ValidateRegisterFields();

        ShowLoginPanel();
    }

    public void OnLoginButtonPressed()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        AuthManager.Instance.Login(email, password);
    }

    public void OnRegisterButtonPressed()
    {
        string email = registerEmailInput.text;
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;
        string dateOfBirth = registerDateOfBirthInput.text;

        AuthManager.Instance.Register(username, email, password, dateOfBirth);
    }

    private void ValidateLoginFields()
    {
        bool isEmailValid = InputValidator.IsValidEmail(emailInput.text);
        bool isPasswordValid = InputValidator.IsValidPassword(passwordInput.text);

        loginButton.interactable = isEmailValid && isPasswordValid;
    }

    public void ValidateRegisterFields()
    {
        bool isEmailValid = InputValidator.IsValidEmail(registerEmailInput.text);
        bool isUsernameValid = InputValidator.IsValidUsername(registerUsernameInput.text);
        bool isPasswordValid = InputValidator.IsValidPassword(registerPasswordInput.text);
        bool doPasswordsMatch = registerPasswordInput.text == registerConfirmPasswordInput.text;
        bool isDateValid = !string.IsNullOrWhiteSpace(registerDateOfBirthInput.text);

        registerButton.interactable = isEmailValid && isUsernameValid && isPasswordValid && doPasswordsMatch && isDateValid;
    }


    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

}
