using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicViewItemsController))]
public class BasicViewItems : MonoBehaviour, IBasicViewItems
{
    private BasicViewItemsController _controller;
    
    public void SetItems(List<InventoryItem> items)
    {
        _controller.UpdateItems(items);
    }

    public InventoryItem GetCurrentItem()
    {
        return _controller.GetCurrentItem();
    }

    private void Awake()
    {
        _controller = gameObject.GetComponent<BasicViewItemsController>();
    }
}