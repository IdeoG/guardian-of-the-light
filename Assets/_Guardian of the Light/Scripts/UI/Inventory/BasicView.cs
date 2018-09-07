using System.Collections.Generic;
using UnityEngine;

public class BasicView : MonoBehaviour
{
    [SerializeField] private GameObject _evenInventoryItems;

    [SerializeField] private GameObject _inventoryArrows;
    private List<Item> _items;

    private InventoryItems _itemsBehaviour;
    [SerializeField] private GameObject _oddInventoryItems;

    private void SetEvenPattern(List<Item> items)
    {
        _oddInventoryItems.SetActive(false);
        _evenInventoryItems.SetActive(true);

        _itemsBehaviour = _evenInventoryItems.GetComponent<InventoryItems>();
        _itemsBehaviour.SetItems(items);
    }

    private void SetOddPattern(List<Item> items)
    {
        _oddInventoryItems.SetActive(true);
        _evenInventoryItems.SetActive(false);

        _itemsBehaviour = _oddInventoryItems.GetComponent<InventoryItems>();
        _itemsBehaviour.SetItems(items);
    }

    private void OnEnable()
    {
        var items = InventorySystem.Instance.GetTookItems();
        _items = items;

        _inventoryArrows.SetActive(items.Count > 5);
        if (items.Count % 2 == 0 && items.Count < 5)
            SetEvenPattern(items);
        else
            SetOddPattern(items);
    }

    public Item GetCurrentItem()
    {
        var ind = _itemsBehaviour.GetCurrentPosition();
        return _items[ind];
    }
}