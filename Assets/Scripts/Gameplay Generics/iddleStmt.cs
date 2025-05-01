using UnityEngine;

public class iddleStmt : IPetStatement
{
    public void EnterState()
    {
        Debug.Log("La mascota está en modo Idle");
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
