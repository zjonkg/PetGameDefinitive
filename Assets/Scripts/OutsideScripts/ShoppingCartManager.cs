using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

public class ShoppingCartManager : MonoBehaviour
{
    public TextMeshProUGUI totalText;
    public Button buyButton;

    private Dictionary<ItemData, int> cart = new Dictionary<ItemData, int>();
    private List<CartItem> cartItemsForSend = new List<CartItem>();

    [System.Serializable]
    public class CartItem
    {
        public string name;
        public int quantity;
        public int price;
        public int totalPrice => quantity * price;
    }

    [System.Serializable]
    public class CartPayload
    {
        public List<CartItem> items;
        public int grandTotal;
    }

    void Awake()
    {
        ItemShop.OnItemUpdated += UpdateCart;
        buyButton.onClick.AddListener(SendToServer);
    }

    void OnDestroy()
    {
        ItemShop.OnItemUpdated -= UpdateCart;
        buyButton.onClick.RemoveListener(SendToServer);
    }

    void UpdateCart(ItemData item, int quantity)
    {
        if (cart.ContainsKey(item))
        {
            cart[item] = quantity;
        }
        else
        {
            cart.Add(item, quantity);
        }

        UpdateTotal();
        PrepareCartForSend();
    }

    void UpdateTotal()
    {
        int total = 0;
        foreach (var pair in cart)
        {
            total += pair.Key.price * pair.Value;
        }
        totalText.text = $"Total: ${total}";
    }

    void PrepareCartForSend()
    {
        cartItemsForSend.Clear();
        foreach (var pair in cart)
        {
            if (pair.Value > 0)
            {
                cartItemsForSend.Add(new CartItem()
                {
                    name = pair.Key.name,
                    quantity = pair.Value,
                    price = pair.Key.price
                });
            }
        }
    }

    void SendToServer()
    {
        if (cartItemsForSend.Count == 0)
        {
            Debug.LogWarning("El carrito está vacío.");
            return;
        }

        int grandTotal = 0;
        foreach (var item in cartItemsForSend)
        {
            grandTotal += item.totalPrice;
        }

        CartPayload payload = new CartPayload
        {
            items = cartItemsForSend,
            grandTotal = grandTotal
        };

        string jsonToSend = JsonConvert.SerializeObject(payload, Formatting.Indented);
        Debug.Log("Datos a enviar al servidor:\n" + jsonToSend);

        // Aquí puedes hacer una llamada HTTP POST a tu endpoint:
        // StartCoroutine(SendCartToServer(jsonToSend));
    }
}
