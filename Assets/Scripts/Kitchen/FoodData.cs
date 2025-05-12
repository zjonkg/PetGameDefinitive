using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class FoodData : ScriptableObject
{
    public int itemId;
    public string itemName;
    public GameObject itemPrefab;
    public int quantity;
}

[System.Serializable]
public class UserItem
{
    public int item_id;
    public string name;
    public int quantity;
}

public class ConsumeItemResponse
{
    public string message;
}

