using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;


    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnDropiePress();
        });
    }
    public void OnDropiePress()
    {
        Debug.Log("Boton funcionando");
    }


}
