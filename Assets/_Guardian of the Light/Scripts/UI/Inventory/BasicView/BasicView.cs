using System.Collections.Generic;
using UnityEngine;

public class BasicView : MonoBehaviour
{
    [SerializeField] private GameObject _oddInventoryItems;
    [SerializeField] private GameObject _evenInventoryItems;

    private List<InventoryItem> _items;
    private InventoryItems _itemsBehaviour;

    private void OnEnable()
    {
        _items = FetchInventoryItems();
    }

    private List<InventoryItem> FetchInventoryItems()
    {
        var items = InventorySystem.Instance.GetTookItems();
        if (items.Count == 0)
        {
            transform.parent.gameObject.SetActive(false);
            return null;
        }


        if (items.Count % 2 == 0 && items.Count < 5)
            SetEvenPattern(items);
        else
            SetOddPattern(items);

        return items;
    }

    private void SetEvenPattern(List<InventoryItem> items)
    {
        _oddInventoryItems.SetActive(false);
        _evenInventoryItems.SetActive(true);

        _itemsBehaviour = _evenInventoryItems.GetComponent<InventoryItems>();
        _itemsBehaviour.SetItems(items);
        
    }

    private void SetOddPattern(List<InventoryItem> items)
    {
        _oddInventoryItems.SetActive(true);
        _evenInventoryItems.SetActive(false);

        var itemsController = _oddInventoryItems.GetComponent<BasicViewItemsController>();
        var itemsBehaviour = _oddInventoryItems.GetComponent<BasicViewItems>();
        itemsBehaviour.SetItems(items);
    }

    public InventoryItem GetCurrentItem()
    {
        return _oddInventoryItems.GetComponent<BasicViewItems>().GetCurrentItem();
    }
    
}