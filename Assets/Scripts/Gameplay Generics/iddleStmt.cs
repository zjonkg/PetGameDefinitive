using UnityEngine;

public class iddleStmt : IPetStatement
{
    [SerializeField] Animator animator;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private PetManager petManager;
    private MonoBehaviour mono;
    public iddleStmt(){
    }

    public iddleStmt(GameObject gameplayUI, Animator animator, PetManager petManager, MonoBehaviour mono)
    {
        this.animator = animator;
        this.gameplayUI = gameplayUI;
        this.petManager = petManager;   
        this.mono = mono;

    }
    public void EnterState()
    {
        gameplayUI.SetActive(true);
        Debug.Log("La mascota está en modo Idle");
        animator.Play("iddle");
        
    }

    public void ExitState()
    {
        Debug.Log("Saliendo de Idle");
    }

    public void UpdateState()
    {
       
        
    }

    
}
