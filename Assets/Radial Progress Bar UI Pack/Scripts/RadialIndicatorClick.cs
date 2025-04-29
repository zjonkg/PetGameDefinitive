using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class RadialIndicatorClick : MonoBehaviour
{
    [Header("Radial Timer Settings")]
    [SerializeField] private float fillDuration = 5.0f; // Tiempo en segundos para llenar
    [SerializeField] private Image radialIndicator = null;
    [SerializeField] private UnityEvent onFillComplete = null;

    [Header("Scene Names")]
    private string sceneIfPlayerIdExists = "Bathroom"; // Nombre de la escena si existe player_id
    private string sceneIfPlayerIdMissing = "LoginScreen";   // Nombre de la escena si NO existe player_id

    private float timer = 0f;
    private bool isFilling = true;

    void Start()
    {
        if (radialIndicator != null)
        {
            radialIndicator.fillAmount = 0f; // Empieza vacío
            radialIndicator.enabled = true;
        }
    }

    void Update()
    {
        if (!isFilling || radialIndicator == null)
            return;

        timer += Time.deltaTime;
        radialIndicator.fillAmount = Mathf.Clamp01(timer / fillDuration);

        if (timer >= fillDuration)
        {
            isFilling = false;
            onFillComplete?.Invoke();
            CheckPlayerPrefAndLoadScene();
        }
    }

    private void CheckPlayerPrefAndLoadScene()
    {
        if (PlayerPrefs.HasKey("player_id") && !string.IsNullOrEmpty(PlayerPrefs.GetString("player_id")))
        {
            // Si existe player_id, carga la escena correspondiente
            SceneManager.LoadScene(sceneIfPlayerIdExists);
        }
        else
        {
            // Si NO existe player_id, carga otra escena
            SceneManager.LoadScene(sceneIfPlayerIdMissing);
        }
    }
}
