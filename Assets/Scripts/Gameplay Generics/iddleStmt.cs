using UnityEngine;

public class iddleStmt : IPetStatement
{
    public void EnterState()
    {
        Debug.Log("La mascota est� en modo Idle");
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    
}
