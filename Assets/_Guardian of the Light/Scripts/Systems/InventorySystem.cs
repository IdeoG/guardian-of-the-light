using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private bool _isOpenInventory;

    [SerializeField] private List<InventoryItem> _items;

    public static InventorySystem Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public List<InventoryItem> GetTookItems()
    {
        var items = new List<InventoryItem>();

        foreach (var item in _items)
            if (item.IsTook)
                items.Add(item);

        return items;
    }


    public InventoryItem GetItemByName(string itemName)
    {
        foreach (var item in _items)
            if (itemName == item.Name)
                return item;

        return null;
    }

    public InventoryItem GetItemById(int itemId)
    {
        foreach (var item in _items)
            if (itemId == item.Id)
                return item;

        return null;
    }
}