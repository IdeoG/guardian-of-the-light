using System.Collections.Generic;

interface IInventoryItems
{
    void SetItems(List<InventoryItem> items);
    InventoryItem GetCurrentItem();
}