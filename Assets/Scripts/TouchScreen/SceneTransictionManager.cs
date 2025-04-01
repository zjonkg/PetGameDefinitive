using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup fadePanel; // Panel de fade (CanvasGroup)

    private static SceneTransitionManager instance;
    private bool isTransitioning = false;

    void Awake()
    {
        // Singleton para asegurarse de que solo haya una instancia de la escena de transición
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto cuando cambiemos de escena
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (fadePanel == null)
        {
            fadePanel = FindFadePanel(); // Encuentra el panel de fade si no está asignado
        }
    }

    // Este método se llamará para iniciar la transición
    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    // Corutina que gestiona la transición con fade
    private IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true;

        // Inicia el fade-out (de 1 a 0)
        yield return StartCoroutine(Fade(1));

        // Cambia a la escena de transición (que se encargará de cargar la escena real)
        SceneManager.LoadScene("TransitionScene");

        // Aseguramos que la escena de transición esté completamente cargada
        yield return new WaitForSeconds(0.1f);

        // Ahora cargamos la escena real de manera asincrónica
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // No activamos la escena aún

        // Esperamos hasta que la escena esté completamente cargada
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Activamos la escena cuando esté lista
            }
            yield return null;
        }

        // Pequeño retraso después de cargar la escena real
        yield return new WaitForSeconds(0.5f);

        // Ahora, ejecutamos el fade-in (de 0 a 1)
        yield return StartCoroutine(Fade(0));

        isTransitioning = false;
    }

    // Corutina que hace el fade entre dos valores de alpha (0 y 1)
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadePanel == null) yield break;

        float startAlpha = fadePanel.alpha;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * 2; // Controla la velocidad del fade
            fadePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, time);
            yield return null;
        }

        fadePanel.alpha = targetAlpha;
    }

    // Busca el panel de fade si no está asignado en el inspector
    private CanvasGroup FindFadePanel()
    {
        GameObject panelObject = GameObject.Find("FadePanel");
        if (panelObject != null)
        {
            return panelObject.GetComponent<CanvasGroup>();
        }
        return null;
    }
}
