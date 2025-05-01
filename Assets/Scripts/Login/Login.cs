using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyGame.Auth
{
    [Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;

        public LoginRequest(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }

    public class LoginResponse
    {
        public string id;
    }

    public class RegisterRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string Birthday { get; set; }
        public string phone_number { get; set; }

        public RegisterRequest(string username, string email, string password, string birthday, string phone_number = "")
        {
            this.username = username;
            this.email = email;
            this.password = password;
            this.Birthday = birthday;
            this.phone_number = phone_number;
        }
    }

    public class RegisterResponse
    {
        public string message { get; set; }
    }

    [Serializable]
    public class HTTPValidationError
    {
        public List<ValidationErrorDetail> detail;
    }

    [Serializable]
    public class ValidationErrorDetail
    {
        public List<object> loc; // Puede ser string o integer
        public string msg;
        public string type;
    }


    public class ErrorResponse
    {
        [JsonProperty("detail")]
        public List<ValidationError> detail { get; set; }
    }

    public class ValidationError
    {
        [JsonProperty("loc")]
        public List<object> loc { get; set; } 

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }


}