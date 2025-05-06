using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class FoodData : ScriptableObject
{
    public int itemId;
    public string itemName;
    public GameObject itemPrefab;
}
