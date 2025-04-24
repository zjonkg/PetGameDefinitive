using UnityEngine;

public class ShowerStmt : IPetStatement
{
    private GameObject showerUI;

    Animator animator;

    public ShowerStmt(GameObject showerUI)
    {
        this.showerUI = showerUI;
    }

    public void EnterState()
    {
        Debug.Log("Entrando al estado Shower");
        showerUI.SetActive(true);
       
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

}
