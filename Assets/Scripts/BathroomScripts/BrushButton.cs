using UnityEngine;
using UnityEngine.UI;

public class BrushButton : MonoBehaviour
{
    [SerializeField] private PetManager petManager;

    private Button btn;

    private void Start()
    {
        IPetStatement currentState = petManager.GetCurrentState();
        
        btn = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(petManager.GetBrushedState());
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

