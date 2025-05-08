using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigurationButton : MonoBehaviour
{
    public void GoToConfiguration()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("PreviousSceneBeforeSettings", currentScene.name);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Configuration"); 
    }
}