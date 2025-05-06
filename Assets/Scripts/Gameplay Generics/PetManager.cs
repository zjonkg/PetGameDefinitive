using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField] private  GameObject gameplayUI;
    [SerializeField] private  PetManager petManager;
    [SerializeField] private  Animator petAnim;
    [SerializeField] private Animator brushAnim;
    private  MonoBehaviour mono;

    static ShowerStmt showerStmt;
    static iddleStmt idleStmt;
    static BrushedStmt brushedStmt;

    private void Awake()
    {
        showerStmt = new ShowerStmt(gameplayUI, petAnim, petManager, this);
        idleStmt = new iddleStmt(gameplayUI, petAnim, petManager, this);
        brushedStmt = new BrushedStmt(gameplayUI, brushAnim, petManager, this);
    }

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

    //Getting statements! 
    public ShowerStmt GetShowerState() => showerStmt;
    public iddleStmt GetIdleState() => idleStmt;

    public BrushedStmt GetBrushedState() => brushedStmt;

    void Update()
    {
        if(currentStatement != null)
        {
            currentStatement.UpdateState();
        }
    }

    

    public void GoToShower()
    {
        changeState(showerStmt);
    }

    
}
