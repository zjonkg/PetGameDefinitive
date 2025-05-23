using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class ItemData
{
    public int id;
    public string name;
    public string description;
    public int price;
}


[System.Serializable]
public class ItemListResponse
{
    public List<ItemData> items;
}


public class FoodShopManager : MonoBehaviour
{
    public GameObject itemShopPrefab;
    public Transform itemsParent;
    private string apiUrl = "https://api-management-pet-production2.up.railway.app/items/";

    void Start()
    {
        StartCoroutine(HttpService.Instance.SendRequest<List<ItemData>>(
            apiUrl,
            "GET",
            null,
            OnItemsReceived,
            OnRequestError
        ));
    }

    void OnItemsReceived(List<ItemData> items)
    {
        foreach (var item in items)
        {
            GameObject go = Instantiate(itemShopPrefab, itemsParent);
            ItemShop component = go.GetComponent<ItemShop>();
            if (component != null)
            {
                component.Initialize(item);
            }
        }
    }

    void OnRequestError(string error)
    {
        Debug.LogError("Error al obtener los ítems: " + error);
    }
}
