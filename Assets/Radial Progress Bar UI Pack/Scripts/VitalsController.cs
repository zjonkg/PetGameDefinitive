using UnityEngine;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [Header("Thirst Values")]
    [SerializeField] private float currentThirst = 100.0f;
    [SerializeField] private float maxThirst = 100.0f;

    [Header("Bottle UI")]
    [SerializeField] private Image drinksBottleUI = null;

    [Header("Drink Variables")]
    [SerializeField] private float amountToDrink = 20.0f;

    public void DrinkAmount()
    {
        if (currentThirst < maxThirst)
        {
            currentThirst = currentThirst + amountToDrink;
            UpdateThirst();
        }
    }

    void UpdateThirst()
    {
        drinksBottleUI.fillAmount = currentThirst / maxThirst;
    }
}
