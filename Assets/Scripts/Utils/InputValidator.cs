using System.Text.RegularExpressions;

public static class InputValidator
{
    // Validaci�n de correo electr�nico
    public static bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Validaci�n de contrase�a (m�nimo 6 caracteres, al menos una letra y un n�mero)
    public static bool IsValidPassword(string password)
    {
        return password.Length >= 8 &&
               Regex.IsMatch(password, @"[a-zA-Z]") &&
               Regex.IsMatch(password, @"[0-9]");
    }

    // Validaci�n de nombre de usuario (m�nimo 3 caracteres)
    public static bool IsValidUsername(string username)
    {
        return username.Length >= 3;
    }
}
