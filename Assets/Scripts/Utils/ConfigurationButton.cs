using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigurationButton : MonoBehaviour
{
    public void GoToConfiguration()
    {
        SceneManager.LoadScene("Configuration"); 
    }
}