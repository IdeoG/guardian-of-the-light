using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicViewBehaviour : MonoBehaviour {

	[SerializeField] private GameObject _inventoryArrows;
	[SerializeField] private GameObject _oddIventoryItems;
	[SerializeField] private GameObject _evenIventoryItems;

	private InventoryItemsBehaviour _itemsBehaviour;
	private List<Item> _items;

	private void SetEvenPattern(List<Item> items)
	{
		_oddIventoryItems.SetActive(false);
		_evenIventoryItems.SetActive(true);
        
		_itemsBehaviour = _evenIventoryItems.GetComponent<InventoryItemsBehaviour>();
		_itemsBehaviour.SetItems(items);
        
	}

	private void SetOddPattern(List<Item> items)
	{
		_oddIventoryItems.SetActive(true);
		_evenIventoryItems.SetActive(false);

		_itemsBehaviour = _oddIventoryItems.GetComponent<InventoryItemsBehaviour>();
		_itemsBehaviour.SetItems(items);
	}

	private void OnEnable()
	{
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
