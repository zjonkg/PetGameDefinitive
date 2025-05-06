using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<FoodData> items;

    public FoodData GetItemById(int id)
    {
        return items.Find(i => i.itemId == id);
    }
}
