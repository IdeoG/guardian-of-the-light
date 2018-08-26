using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private bool _isOpenInventory;

    [SerializeField] private List<Item> _items;


    private void Awake()
    {
        Instance = this;
    }

    public static InventorySystem Instance { get; private set; }

    public List<Item> GetItems()
    {
        var items = new List<Item>();

        foreach (var item in _items)
        {
            if (item.IsTook)
            {
                items.Add(item);
            }
        }

        return items;
    }
}