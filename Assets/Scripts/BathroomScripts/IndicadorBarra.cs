using UnityEngine;
using UnityEngine.UI;

public class IndicadorBarra : MonoBehaviour
{
    [SerializeField] Image fillBar;
    [SerializeField] PetManager petManager;
    private float barMaxValue = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillBar.fillAmount = 100f;
    }
}
