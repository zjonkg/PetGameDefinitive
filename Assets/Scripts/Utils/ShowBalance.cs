using UnityEngine;
using UnityEngine.UI;

public class ShowBalance : MonoBehaviour
{
    public Text balanceText;
    private string lastBalance;

    void Start()
    {
        lastBalance = PlayerPrefs.GetString("balance", "0");
        UpdateBalanceText(lastBalance);
    }

    void Update()
    {
        string currentBalance = PlayerPrefs.GetString("balance", "0");

        if (currentBalance != lastBalance)
        {
            lastBalance = currentBalance;
            UpdateBalanceText(currentBalance);
        }
    }

    void UpdateBalanceText(string balance)
    {
        if (balanceText != null)
        {
            balanceText.text = balance;
        }
    }
}
