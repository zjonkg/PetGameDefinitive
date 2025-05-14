using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            Scene initialScene = SceneManager.GetActiveScene();
            SaveCurrentSceneInfo(initialScene);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Configuration")
        {
            SaveCurrentSceneInfo(scene);
        }
    }

    private void SaveCurrentSceneInfo(Scene scene)
    {
        PlayerPrefs.SetString("SceneName", scene.name);
        PlayerPrefs.SetInt("SceneIndex", scene.buildIndex);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}