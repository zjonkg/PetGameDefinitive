using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using WCP;

public class LeungRequestUI : MonoBehaviour
{
    [Header("Railway Configuration")]
    [SerializeField] private string railwayUrl = "https://iapetv3-production.up.railway.app/chat"; 
    [SerializeField] private string sessionId = "3"; 

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    public WChatPanel wcp;

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClick);
    }

    private void OnSendButtonClick()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            string userMessage = inputField.text;
            MostrarBurbuja(userMessage, true);
            StartCoroutine(SendToRailwayAPI(userMessage));
            inputField.text = "";
        }
    }

    IEnumerator SendToRailwayAPI(string userMessage)
    {
  
        LeungRequest requestData = new LeungRequest
        {
            content = userMessage,
            session_id = sessionId
        };
        string jsonData = $"{{\"content\":\"{EscapeJson(userMessage)}\",\"session_id\":\"{EscapeJson(sessionId)}\"}}";


        using (UnityWebRequest request = new UnityWebRequest(railwayUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
               
                string rawJson = request.downloadHandler.text;
                rawJson = rawJson.Replace("session_id", "sessionId");

                LeungAPIResponse response = JsonUtility.FromJson<LeungAPIResponse>(rawJson);
                MostrarBurbujaIA(response.response, false);
            }
            else
            {
                string errorMsg = request.error;
                if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
                {
                    errorMsg += $"\nRespuesta del servidor: {request.downloadHandler.text}";
                }
                MostrarBurbujaIA($"Leung está hibernando... (Error: {errorMsg})", false);
                Debug.LogError($"Error: {errorMsg}");
            }
        }
    }

    private void MostrarBurbuja(string texto, bool esUsuario)
    {
        wcp.AddChatAndUpdate(!esUsuario, texto + " ", 1);
    }

    private void MostrarBurbujaIA(string texto, bool esUsuario)
    {
        wcp.AddChatAndUpdate(!esUsuario, texto + " ", 0);
    }

    [Serializable]
    public class LeungRequest
    {
        public string content;
        public string session_id;
    }

    [Serializable]
    private class LeungAPIResponse
    {
        public string response;
        public string sessionId;
    }

    private string EscapeJson(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

}
