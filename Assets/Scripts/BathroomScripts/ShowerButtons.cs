using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;

    
    public PetManager petManager;
    public GameObject showerUI;


    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(new ShowerStmt(showerUI));
        });
    }
    public void OnDropiePress()
    {
        petManager.changeState(new ShowerStmt(showerUI));
        Debug.Log("Boton funcionando");
    }


}
