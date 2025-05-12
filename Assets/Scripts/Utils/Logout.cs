using UnityEngine;
using UnityEngine.SceneManagement;

public class  Logout : MonoBehaviour
{
    public void SetLogout()
    {
        PlayerPrefs.SetString("player_id", "");
        SceneManager.LoadSceneAsync("LoginScreen");
    }
}
