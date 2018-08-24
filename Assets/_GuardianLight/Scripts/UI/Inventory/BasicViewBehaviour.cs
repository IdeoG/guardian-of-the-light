using System.Collections.Generic;
using UnityEngine;

public class BasicViewBehaviour : MonoBehaviour {

	[SerializeField] private GameObject _inventoryArrows;
	[SerializeField] private GameObject _oddInventoryItems;
	[SerializeField] private GameObject _evenInventoryItems;

	private InventoryItemsBehaviour _itemsBehaviour;
	private List<Item> _items;

	private void SetEvenPattern(List<Item> items)
	{
		_oddInventoryItems.SetActive(false);
		_evenInventoryItems.SetActive(true);
        
		_itemsBehaviour = _evenInventoryItems.GetComponent<InventoryItemsBehaviour>();
		_itemsBehaviour.SetItems(items);
        
	}

	private void SetOddPattern(List<Item> items)
	{
		_oddInventoryItems.SetActive(true);
		_evenInventoryItems.SetActive(false);

		_itemsBehaviour = _oddInventoryItems.GetComponent<InventoryItemsBehaviour>();
		_itemsBehaviour.SetItems(items);
	}

	private void OnEnable()
	{
		Debug.Log($"BasicViewBehaviour: OnEnable");
		var items = InventorySystem.Instance.GetItems();
		_items = items;
		
		_inventoryArrows.SetActive(items.Count > 5);
		if (items.Count % 2 == 0 && items.Count < 5)
		{
			SetEvenPattern(items);
		}
		else
		{
			SetOddPattern(items);
		}

	}

	public Item GetCurrentItem()
	{
		var ind = _itemsBehaviour.GetCurrentPosition();
		return _items[ind];
	}
}
