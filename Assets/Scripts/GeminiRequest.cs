using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System;
using WCP;
using System.Linq;

public class GeminiRequestUI : MonoBehaviour
{
    [Header("API Key")]
    [SerializeField] private string apiKey = "AIzaSyAJVr-hI02-yzG3Utj8yaZTAVDMRJlTFKA";

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    [Header("Chat Content")]
    [SerializeField] private Transform chatContent; // << Scroll View Content aquí
    public WChatPanel wcp;

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClick);
    }

    private void OnSendButtonClick()
    {
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            userInput = userInput + " ";
            MostrarBurbuja(userInput, true); // Mostrar mensaje del usuario
            StartCoroutine(SendGeminiRequest(userInput));
            inputField.text = ""; // Limpiar input
        }
    }

    IEnumerator SendGeminiRequest(string userPrompt)
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
        string jsonData = JsonUtility.ToJson(new RequestWrapper(userPrompt));

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string rawJson = request.downloadHandler.text;
                string generatedText = ExtractTextFromResponse(rawJson);

                MostrarBurbuja(generatedText, false); // Mostrar respuesta de Gemini
            }
            else
            {
                MostrarBurbuja("[Error: " + request.error + "]", false);
            }
        }
    }

    private string ExtractTextFromResponse(string json)
    {
        try
        {
            JObject response = JObject.Parse(json);
            string text = response["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
            return text ?? "[Texto no encontrado]";
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al extraer el texto: " + ex.Message);
            return "[Error al procesar el JSON]";
        }
    }

    private void MostrarBurbuja(string texto, bool esUsuario)
    {
        wcp.AddChatAndUpdate(!esUsuario, texto, 1);
    }

    // Clases auxiliares para JSON
    [System.Serializable]
    public class Part
    {
        public string text;
        public Part(string text) => this.text = text;
    }

    [System.Serializable]
    public class Content
    {
        public Part[] parts;
        public Content(string text) => parts = new Part[] { new Part(text) };
    }

    [System.Serializable]
    public class RequestWrapper
    {
        public Content[] contents;
        public RequestWrapper(string text) => contents = new Content[] { new Content(text) };
    }
}
