using System.Collections.Generic;

interface IBasicViewItems
{
    void SetItems(List<InventoryItem> items);
    InventoryItem GetCurrentItem();
}