using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement; 

public class RadialIndicatorClick : MonoBehaviour
{
    [Header("Radial Timer Settings")]
    [SerializeField] private float fillDuration = 5.0f; 
    [SerializeField] private Image radialIndicator = null;
    [SerializeField] private UnityEvent onFillComplete = null;

    [Header("Scene Names")]
    private string sceneIfPlayerIdExists = "Bathroom"; 
    private string sceneIfPlayerIdMissing = "LoginScreen";   

    private float timer = 0f;
    private bool isFilling = true;

    void Start()
    {
        if (radialIndicator != null)
        {
            radialIndicator.fillAmount = 0f; 
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
            SceneManager.LoadScene(sceneIfPlayerIdExists);
        }
        else
        {
            SceneManager.LoadScene(sceneIfPlayerIdMissing);
        }
    }
}
