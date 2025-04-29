using System.Collections;
using UnityEngine;

public class ShowerStmt : IPetStatement
{
    private GameObject showerUI;

    Animator animator;
    private MonoBehaviour mono; // para usar Coroutine
    
    public GameObject player;
    PetManager petManager;

    public ShowerStmt(GameObject showerUI, Animator animator, MonoBehaviour mono, PetManager petManager)
    {
        this.showerUI = showerUI;
        this.animator = animator;
        this.mono = mono;
        
    }

    public void EnterState()
    {
        Debug.Log("Entrando al estado Shower");
        showerUI.SetActive(true);
        animator.Play("walkingtest");
       
    }

    public void ExitState()
    {
        Debug.Log("Saliendo del estado Shower");
        showerUI.SetActive(false);
    }

    public void UpdateState()
    {
        //
    }

    private IEnumerator VolverAIdle()
    {
        yield return new WaitForSeconds(5f); // duración de la animación
        petManager.changeState(new iddleStmt());
    }

}
