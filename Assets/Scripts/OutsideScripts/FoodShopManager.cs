using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class ItemData
{
    public string name;
    public int price;
    public string imagePath;
}

public class FoodShopManager : MonoBehaviour
{
    public GameObject itemShopPrefab; // Arrastra tu prefab aquí
    public Transform itemsParent;     // Contenedor donde se generarán los ítems

    void Start()
    {
        // Tu JSON como cadena (puedes cargarlo desde archivo también)
        string json = @"
        [
    {
        ""name"": ""Cerdo"",
        ""price"": 3,
        ""imagePath"": ""Imgs/pig""
    },
    {
        ""name"": ""Águila"",
        ""price"": 5,
        ""imagePath"": ""Imgs/eagle""
    },
    {
        ""name"": ""Zombi"",
        ""price"": 7,
        ""imagePath"": ""Imgs/zombie""
    },
    {
        ""name"": ""Zombi"",
        ""price"": 7,
        ""imagePath"": ""Imgs/zombie""
    },
    {
        ""name"": ""Cerdo"",
        ""price"": 3,
        ""imagePath"": ""Imgs/pig""
    },
    {
        ""name"": ""Águila"",
        ""price"": 5,
        ""imagePath"": ""Imgs/eagle""
    },
    {
        ""name"": ""Zombi"",
        ""price"": 7,
        ""imagePath"": ""Imgs/zombie""
    },
    {
        ""name"": ""Zombi"",
        ""price"": 7,
        ""imagePath"": ""Imgs/zombie""
    }
]
";

        List<ItemData> items = JsonConvert.DeserializeObject<List<ItemData>>(json);

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
}