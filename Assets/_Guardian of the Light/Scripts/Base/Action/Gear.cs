public class Gear : BaseAction
{
	private string _itemName = "Шестерня";
	
	protected override void OnKeyActionPressedDown()
	{
		CollectGear();
	}

	private void CollectGear()
	{
		var item = InventorySystem.Instance.GetItemByName(_itemName);
		item.IsTook = true;

		Destroy(gameObject);
	}
}
