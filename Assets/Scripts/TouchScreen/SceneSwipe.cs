using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwipe : MonoBehaviour
{
    public float swipeThreshold = 1000f;
    public CanvasGroup fadePanel;
    private Vector2 startTouchPosition;
    private bool isTransitioning = false;

    private static SceneSwipe instance;

    void Awake()
    {

        swipeThreshold = Screen.width * 0.25f; 
        // Singleton para evitar duplicados
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Asegura que el fadePanel se mantenga en todas las escenas
        if (fadePanel == null)
        {
            fadePanel = FindFadePanel();
        }

        if (fadePanel != null)
        {
            DontDestroyOnLoad(fadePanel.gameObject);
        }
    }

    void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
        if (isTransitioning) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 endTouchPosition = touch.position;
                Vector2 swipeDelta = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > swipeThreshold)
                {
                    if (swipeDelta.x > 0)
                        LoadPreviousScene();
                    else
                        LoadNextScene();
                }
            }
        }
    }


    void LoadNextScene()
    {
        string nextScene = GetNextScene();
        if (!string.IsNullOrEmpty(nextScene))
            StartCoroutine(FadeAndLoadScene(nextScene, false)); // Swipe izquierda ➡
    }

    void LoadPreviousScene()
    {
        string prevScene = GetPreviousScene();
        if (!string.IsNullOrEmpty(prevScene))
            StartCoroutine(FadeAndLoadScene(prevScene, true)); // Swipe derecha ⬅
    }

    string GetNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "House") return "Hall2";
        if (currentScene == "Hall2") return "Bathroom";
        if(currentScene == "Bathroom") return "Kitchen";
        return "";
    }

    string GetPreviousScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Kitchen") return "Bathroom";
        if (currentScene == "Bathroom") return "Hall2";
        if (currentScene == "Hall2") return "House";
        return "";
    }

    IEnumerator FadeAndLoadScene(string sceneName, bool isRightSwipe)
    {
        isTransitioning = true;

        // Diferente fade-out según la dirección del swipe
        if (isRightSwipe)
        {
            yield return StartCoroutine(SlideFade(1, Vector2.right)); // Fade con deslizamiento hacia la derecha
        }
        else
        {
            yield return StartCoroutine(SlideFade(1, Vector2.left)); // Fade con deslizamiento hacia la izquierda
        }

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.5f);

        fadePanel = FindFadePanel(); // Asegura que siempre tengamos el fadePanel

        // Fade-in en dirección opuesta al anterior
        if (isRightSwipe)
        {
            yield return StartCoroutine(SlideFade(0, Vector2.left));
        }
        else
        {
            yield return StartCoroutine(SlideFade(0, Vector2.right));
        }

        isTransitioning = false;
    }

    IEnumerator SlideFade(float targetAlpha, Vector2 direction)
    {
        if (fadePanel == null) yield break;

        float startAlpha = fadePanel.alpha;
        float time = 0;
        RectTransform fadeRect = fadePanel.GetComponent<RectTransform>();
        Vector2 startPos = fadeRect.anchoredPosition;
        Vector2 endPos = startPos + (direction * 300); // Deslizamiento lateral

        while (time < 1)
        {
            time += Time.deltaTime * 2;
            fadePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, time);
            fadeRect.anchoredPosition = Vector2.Lerp(startPos, endPos, time);
            yield return null;
        }

        fadePanel.alpha = targetAlpha;
        fadeRect.anchoredPosition = startPos;
    }

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
