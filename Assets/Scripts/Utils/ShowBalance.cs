using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowBalance : MonoBehaviour
{
    public Text balanceText;

    void Start()
    {
        string balance = PlayerPrefs.GetString("balance", "0");

        if (balanceText != null)
        {
            balanceText.text = balance;
        }
    }
}
