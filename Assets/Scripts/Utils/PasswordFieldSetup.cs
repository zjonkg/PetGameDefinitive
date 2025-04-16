using UnityEngine;
using TMPro;

public class PasswordFieldSetup : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordInputField2;

    void Start()
    {
        // Configuramos el InputField para que sea un campo de contraseña
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.asteriskChar = '*';

        passwordInputField2.contentType = TMP_InputField.ContentType.Password;
        passwordInputField2.asteriskChar = '*';
    }
}
