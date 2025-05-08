using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopContainer; 

    public void ToggleShop()
    {
        if (shopContainer != null)
        {
            shopContainer.SetActive(!shopContainer.activeSelf);
            Debug.Log("Tienda " + (shopContainer.activeSelf ? "abierta" : "cerrada"));
        }
    }
}