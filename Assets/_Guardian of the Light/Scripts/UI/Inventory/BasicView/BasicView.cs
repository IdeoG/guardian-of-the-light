using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicView : MonoBehaviour
{
    [SerializeField] private GameObject _oddInventoryItems;
    [SerializeField] private GameObject _evenInventoryItems;

    public InventoryItem GetCurrentItem()
    {
        return _oddInventoryItems.GetComponent<BasicViewItems>().GetCurrentItem();
    }
    
    private void OnEnable()
    {
        FetchInventoryItems();
    }

    private void SetEvenPattern(List<InventoryItem> items)
    {
        throw new NotImplementedException();
    }

    private void SetOddPattern(List<InventoryItem> items)
    {
        _oddInventoryItems.SetActive(true);
        _evenInventoryItems.SetActive(false);

        var itemsBehaviour = _oddInventoryItems.GetComponent<BasicViewItems>();
        itemsBehaviour.SetItems(items);
    }

    private void FetchInventoryItems()
    {
        var items = InventorySystem.Instance.GetTookItems();
        if (items.Count == 0)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        if (items.Count % 2 == 0 && items.Count < 5)
            SetEvenPattern(items);
        else
            SetOddPattern(items);
    }
}