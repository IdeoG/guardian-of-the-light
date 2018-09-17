using UnityEngine;

public class CollectableItem : BaseAction
{
    [SerializeField] private string _itemName;

    protected override void OnKeyActionPressedDown()
    {
        CollectItem();
    }

    private void CollectItem()
    {
        var item = InventorySystem.Instance.GetItemByName(_itemName);
        item.IsTook = true;

        Destroy(gameObject);
    }
}