using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
       public void ReturnToPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString("SceneName", "MainMenu"); 
        SceneManager.LoadScene(previousScene);
    }

}
