using UnityEngine;
using UnityEngine.UI;

public class ShowerButtons : MonoBehaviour
{
 
    Animator animator;

    
    [SerializeField] private PetManager petManager;
    [SerializeField] private GameObject showerUI;
    [SerializeField] private Animator petAnim;

    Button btn;


    private void Start()
    {
        btn = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(petManager.GetShowerState());
        });
    }

    private void Update()
    {
        // Check if the current state is BrushedStmt
        if (petManager.GetCurrentState() is iddleStmt)
        {
            // Disable the button if the current state is BrushedStmt
            btn.interactable = true;
        }
        else
        {
            // Enable the button otherwise
            btn.interactable = false;
        }
    }

}
