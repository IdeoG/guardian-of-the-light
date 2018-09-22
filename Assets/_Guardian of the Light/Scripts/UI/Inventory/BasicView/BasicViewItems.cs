using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicViewItemsController))]
public class BasicViewItems : MonoBehaviour, IInventoryItems
{
    private BasicViewItemsController _controller;
    private BasicViewItemsController Controller
    {
        get
        {
            if (_controller == null)
            {
                _controller = GetComponent<BasicViewItemsController>();
            }

            return _controller;
        }
    }

    public void SetItems(List<InventoryItem> items)
    {
        Controller.UpdateItems(items);
    }

    public InventoryItem GetCurrentItem()
    {
        return Controller.GetCurrentItem();
    }

}