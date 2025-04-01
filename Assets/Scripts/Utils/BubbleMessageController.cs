using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MessageBubble : MonoBehaviour
{
    public Transform target;  // Personaje objetivo
    public Vector3 offset = new Vector3(1, 2, 0); // Posición relativa (derecha-superior)
    public TextMeshProUGUI messageText; // Texto del mensaje
    public CanvasGroup canvasGroup; // Para el fade-out
    public float displayTime = 2f; // Tiempo antes de que desaparezca
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }

    public void ShowMessage(string message, Transform newTarget)
    {
        target = newTarget;
        messageText.text = message;
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.zero; // Empieza pequeño

        StopAllCoroutines(); // Detener cualquier animación previa
        StartCoroutine(AnimateScale(Vector3.one, 0.3f)); // Animar aparición

        StartCoroutine(HideAfterTime(displayTime)); // Desaparecer después de `displayTime` segundos
    }

    private IEnumerator AnimateScale(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeOut(0.5f)); // Inicia el FadeOut
    }

    private IEnumerator FadeOut(float duration)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
