using System.Collections;
using UnityEngine;

public class BrushedStmt : IPetStatement
{
    private GameObject showerUI;
    Animator animator;
    private MonoBehaviour mono; // Coroutine!! 
    PetManager petManager;

    public BrushedStmt(GameObject showerUI, Animator animator, PetManager petManager, MonoBehaviour mono, Animator petAnim)
    {
        this.showerUI = showerUI;
        this.animator = animator;
        this.mono = mono;
        this.petManager = petManager;
    }
    public void EnterState()
    {
        Debug.Log("Entrando al estado Brushed");
        animator.Play("BrushButton");
        mono.StartCoroutine(brushToIdle()); // Start the coroutine to play shower animation
    }

    public void ExitState()
    {   

        Debug.Log("Saliendo del estado Brushed");
    }

    public void UpdateState()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator brushToIdle()
    {
        yield return new WaitForSeconds(5f); // duración de la animación
        petManager.changeState(petManager.GetIdleState());
    }

}
