﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Все предметы инвентаря")] [SerializeField]
    private List<Item> _items;

    private IDisposable _inventoryAction;
    private IDisposable _inspectAction;

    private bool _isOpenInventory;
    private bool _isOpenInspectView;
    /** TODO: InventorySystem
     * 1. Добавить обработку нажатия кнопки инвенторя
     * 2. Добавить метод для добавления предмета по имени
     * 3. Добавить метод для проверки наличия предмета в инвентаре
     * 4. Добавить метод для отображения всех доступных элементов в инвентаре по нажатию кнопки
     * 5. Добавить обработку нажатия кнопки просмотра предмета
     */

    private void Start()
    {
        _inventoryAction = InputSystem.Instance.KeyInventoryPressed
            .Subscribe(_ => OnKeyInventoryPressed())
            .AddTo(this);
    }

    private void OnKeyInventoryPressed()
    {
        _isOpenInventory = !_isOpenInventory;
        
        if (_isOpenInventory)
        {
            ShowInventory();
        }
        else
        {
            HideInventory();
        }
    }
    
    private void HideInventory()
    {
        _inspectAction.Dispose();
        
        GameManagerSystem.Instance.HideInventoryView();
    }
    
    private void ShowInventory()
    {
        _inspectAction = InputSystem.Instance.KeyInspectPressed
            .Subscribe(_ => OnKeyInspectPressed())
            .AddTo(this);
        
        GameManagerSystem.Instance.ShowInventoryView();
    }

    private void OnKeyInspectPressed()
    {
        _isOpenInspectView = !_isOpenInspectView;

        if (_isOpenInspectView)
        {
            ShowInspectView();
        }
        else
        {
            HideInspectView();
        }
    }

    private void HideInspectView()
    {
        throw new NotImplementedException();
    }

    private void ShowInspectView()
    {
        throw new NotImplementedException();
    }

}