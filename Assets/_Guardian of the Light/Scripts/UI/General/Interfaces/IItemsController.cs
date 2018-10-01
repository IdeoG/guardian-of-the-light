using System.Collections.Generic;

public interface IItemsController
{
    void UpdateItems(List<InventoryItem> items);
    InventoryItem GetCurrentItem();
}