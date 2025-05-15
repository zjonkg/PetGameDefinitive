using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public void ToggleMemoryGame()
    {
        SceneManager.LoadScene("MemoryGame");
    }
    public void ToggleAngryLaura()
    {
        SceneManager.LoadScene("AngryLaura");
    }
}