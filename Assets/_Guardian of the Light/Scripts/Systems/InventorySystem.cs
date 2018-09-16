using System.Collections.Generic;
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
        for (var i = 0; i < _items.Count; i++)
        {
            if (itemName == _items[i].Name)
            {
                return _items[i];
            }
        }

        return null;
    }
}