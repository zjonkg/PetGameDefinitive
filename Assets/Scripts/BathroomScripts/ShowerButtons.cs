using UnityEngine;
using UnityEngine.UI;

public class ShowerButtons : MonoBehaviour
{
 
    Animator animator;

    
    [SerializeField] private PetManager petManager;
    [SerializeField] private GameObject showerUI;
    [SerializeField] private Animator petAnim;


    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(petManager.GetShowerState());
        });
    }

    public void OnDropiePress()
    {
        petManager.changeState(new ShowerStmt(showerUI, petAnim, petManager, this));
        Debug.Log("Boton funcionando");
    }

}
