using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
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
    public class ShoppingCartPayload
    {
        public int user;
        public List<CartItem> item = new List<CartItem>();
        public int totalPrice; // Nuevo campo
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

        ShoppingCartPayload payload = new ShoppingCartPayload();

        if (PlayerPrefs.HasKey("userId"))
        {
            payload.user = PlayerPrefs.GetInt("userId");
        }
        else
        {
            Debug.LogError("No hay un usuario logueado.");
            return;
        }

        payload.item = cartItemsForSend;

        // Calcular el total general
        payload.totalPrice = 0;
        foreach (var item in cartItemsForSend)
        {
            payload.totalPrice += item.totalPrice;
        }

        string jsonToSend = JsonConvert.SerializeObject(payload, Formatting.Indented);
        Debug.Log("Datos a enviar al servidor:\n" + jsonToSend);

        // Opcional: Iniciar corrutina para enviar por HTTP
        // StartCoroutine(SendCartToServer(jsonToSend));
    }
}