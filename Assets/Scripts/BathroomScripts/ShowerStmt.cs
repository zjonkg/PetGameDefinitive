using System.Collections;
using UnityEngine;

public class ShowerStmt : IPetStatement
{
    private GameObject showerUI;

    Animator animator;
    private MonoBehaviour mono; // Coroutine!! 

    public GameObject player;
    PetManager petManager;

    public ShowerStmt(GameObject showerUI, Animator animator, PetManager petManager, MonoBehaviour mono)
    {
        this.showerUI = showerUI;
        this.animator = animator;
        this.mono = mono;
        this.petManager = petManager;

    }

    public void EnterState()
    {
        Debug.Log("Entrando al estado Shower");
        animator.Play("walkingtest");
        showerUI.SetActive(false);
        mono.StartCoroutine(PlayShowerThenIdle()); // Start the coroutine to play shower animation

    }

    public void ExitState()
    {
        animator.Play("showertoidle");
        Debug.Log("Saliendo del estado Shower");
        showerUI.SetActive(true);
    }

    public void UpdateState()
    {
        
    }

    private IEnumerator backToIdle()
    {
        yield return new WaitForSeconds(6f); // duración de la animación
        petManager.changeState(petManager.GetIdleState());
    }

    private IEnumerator PlayShowerThenIdle()
    {
        yield return new WaitForSeconds(4.5f); // Segunda Etapa
        animator.Play("showertoidle");

        yield return new WaitForSeconds(1.5f); 
        petManager.changeState(petManager.GetIdleState());
    }

}
