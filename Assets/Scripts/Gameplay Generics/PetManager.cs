using UnityEngine;

public class PetManager : MonoBehaviour
{
    private IPetStatement currentStatement;
    public void changeState(IPetStatement newStmt)
    {
        if (currentStatement != null)
        {
            currentStatement.ExitState();
        }

        currentStatement = newStmt;

        if (currentStatement != null) { 
            currentStatement.EnterState();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(currentStatement != null)
        {
            currentStatement.UpdateState();
        }
    }
}
