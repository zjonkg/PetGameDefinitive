using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;

    public Button showerButton;

    private void Start()
    {

        showerButton.onClick.AddListener(OnDropiePress);
    }
    public void OnDropiePress()
    {
        Debug.Log("Boton funcionando");
    }


}
