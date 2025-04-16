using TMPro;
using UnityEngine;

public class ErrorMessageUI : MonoBehaviour
{
    public static ErrorMessageUI Instance { get; private set; }

    public GameObject panel;
    public TextMeshProUGUI errorText;

    private void Awake()
    {
        // Singleton pattern básico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }
        Instance = this;
    }

    public void ShowError(string message)
    {
        errorText.text = message;
        panel.SetActive(true);
    }

    public void HideError()
    {
        panel.SetActive(false);
    }
}
