using System.Text.RegularExpressions;

public static class InputValidator
{
    // Validación de correo electrónico
    public static bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Validación de contraseña (mínimo 6 caracteres, al menos una letra y un número)
    public static bool IsValidPassword(string password)
    {
        return password.Length >= 8 &&
               Regex.IsMatch(password, @"[a-zA-Z]") &&
               Regex.IsMatch(password, @"[0-9]");
    }

    // Validación de nombre de usuario (mínimo 3 caracteres)
    public static bool IsValidUsername(string username)
    {
        return username.Length >= 3;
    }
}
