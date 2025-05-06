using UnityEngine;
using UnityEngine.UI;

public class BrushButton : MonoBehaviour
{
    [SerializeField] private PetManager petManager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            petManager.changeState(petManager.GetBrushedState());
        });
    }

}

