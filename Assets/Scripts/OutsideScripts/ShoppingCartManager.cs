using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;

public class ShoppingCartManager : MonoBehaviour
{


    public TextMeshProUGUI totalText;
    public Button buyButton;

    private Dictionary<ItemData, int> cart = new Dictionary<ItemData, int>();
    private List<CartItem> cartItemsForSend = new List<CartItem>();

    [System.Serializable]
    public class CartItem
    {
        public int id;
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
                    id = pair.Key.id,
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

        if (PlayerPrefs.HasKey("player_id"))
        {
            payload.user = int.Parse(PlayerPrefs.GetString("player_id"));
        }
        else
        {
            Debug.LogError("No hay un usuario logueado.");
            return;
        }

        payload.item = cartItemsForSend;

        payload.totalPrice = 0;
        foreach (var item in cartItemsForSend)
        {
            payload.totalPrice += item.totalPrice;
        }

        Debug.Log("Datos a enviar al servidor:\n" + JsonConvert.SerializeObject(payload, Formatting.Indented));

        SendCartToServer(payload);
    }

    private void SendCartToServer(ShoppingCartPayload payload)
    {
        StartCoroutine(HttpService.Instance.SendRequest<CartResponse>(
            "https://api-management-pet-production.up.railway.app/items/buy",
            "PUT",
            payload,
            (response) =>
            {
                Debug.Log("Compra realizada con éxito. Respuesta: " + response.message);
                return;
            },
            (error) =>
            {
                Debug.LogError("Error en la compra: " + error);
            }
        ));
    }

    public class CartResponse
    {
        public string message;
    }


}