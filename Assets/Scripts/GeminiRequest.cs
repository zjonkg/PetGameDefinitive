using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System;

public class GeminiRequestUI : MonoBehaviour
{
    [Header("API Key")]
    [SerializeField] private string apiKey = "AIzaSyAJVr-hI02-yzG3Utj8yaZTAVDMRJlTFKA";

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_Text outputText; // (opcional)

    [Header("ChatBubble Handler")]
    [SerializeField] private GameHandler_ChatBubble chatHandler;

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClick);
    }

    private void OnSendButtonClick()
    {
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            StartCoroutine(SendGeminiRequest(userInput));
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

                Debug.Log("Response: " + generatedText);

                if (outputText != null)
                    outputText.text = generatedText;

                if (chatHandler != null)
                    chatHandler.ShowChatMessage(generatedText);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                if (outputText != null)
                    outputText.text = "Error: " + request.error;
            }
        }
    }

    private string ExtractTextFromResponse(string json)
    {
        try
        {
            // Parseamos el JSON
            JObject response = JObject.Parse(json);

            // Accedemos al primer candidato y extraemos el texto
            string text = response["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            return text ?? "[Texto no encontrado]";
        }
        catch (Exception ex)
        {
            // En caso de error, devolvemos un mensaje de error
            Debug.LogError("Error al extraer el texto: " + ex.Message);
            return "[Error al procesar el JSON]";
        }
    }

    // Clases auxiliares para el JSON de Gemini
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
