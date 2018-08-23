using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvasBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryArrows;
    [SerializeField] private GameObject _oddIventoryItems;
    [SerializeField] private GameObject _evenIventoryItems;

    private InventoryItemsBehaviour _oddItemsBehaviour;
    private InventoryItemsBehaviour _evenItemsBehaviour;
    

    private void Awake()
    {
        _oddItemsBehaviour = _oddIventoryItems.GetComponent<InventoryItemsBehaviour>();
        _evenItemsBehaviour = _evenIventoryItems.GetComponent<InventoryItemsBehaviour>();
    }

    private void OnEnable()
    {
        /** TODO: OnEnable
         * 1. Обращаемся к инвентарю
         * 2. Получаем все подобранные предметы из инвентаря
         * 3.1. Если количество предметов четное, то используем четный паттерн
         * 3.1. Если количество предметов нечетное, то используем нечетный паттерн
         * 4. Заполняем места предметы графического инвентаря спрайтами и цветами
         */

        var inventory = InventorySystem.Instance;

        var items = inventory.GetItems();


        if (items.Count % 2 == 0 && items.Count < 5)
        {
            SetEvenPattern(items);
        }
        else
        {
            SetOddPattern(items);
        }

        _inventoryArrows.SetActive(items.Count > 5);
    }

    private void SetEvenPattern(List<Item> items)
    {
        _oddIventoryItems.SetActive(false);
        _evenIventoryItems.SetActive(true);
        
        _evenItemsBehaviour.SetItems(items);
        
    }

    private void SetOddPattern(List<Item> items)
    {
        _oddIventoryItems.SetActive(true);
        _evenIventoryItems.SetActive(false);

        _oddItemsBehaviour.SetItems(items);
    }
}