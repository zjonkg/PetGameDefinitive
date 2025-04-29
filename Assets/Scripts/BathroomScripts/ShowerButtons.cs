using UnityEngine;
using UnityEngine.UI;

public class ShowerButtons : MonoBehaviour
{
 
    Animator animator;

    
    public PetManager petManager;
    public GameObject showerUI;
    public Animator petAnim;


    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(new ShowerStmt(showerUI, petAnim, this, petManager));
        });
    }

    public void OnDropiePress()
    {
        petManager.changeState(new ShowerStmt(showerUI, petAnim, this, petManager));
        Debug.Log("Boton funcionando");
    }


}
