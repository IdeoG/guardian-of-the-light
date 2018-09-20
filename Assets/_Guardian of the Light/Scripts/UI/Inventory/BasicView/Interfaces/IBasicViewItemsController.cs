using System.Collections.Generic;

public interface IBasicViewItemsController
{
    void OnLeftArrowPressed();
    void OnRightArrowPressed();
    void UpdateItems(List<InventoryItem> items);
}