using System;
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

    public int GetCurrentItemIndex()
    {
        throw new NotImplementedException();
    }


    private void Awake()
    {
        _controller = GetComponent<BasicViewItemsController>();
    }
}