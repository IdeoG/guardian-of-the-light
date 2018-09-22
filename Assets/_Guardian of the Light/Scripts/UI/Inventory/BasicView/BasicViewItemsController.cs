using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BasicViewItemsEffects))]
public class BasicViewItemsController : MonoBehaviour, IItemsController
{
    #region private_inspector_vars

    [SerializeField] private Transform _prefabs2D;
    [SerializeField] private List<RectTransform> _placeholders;

    #endregion

    #region private_vars
        
    private int _itemIndex;
    private int _itemsCount;
    private int _baseItemIndex = 2;
    
    private IDisposable _leftArrowPressDown;
    private IDisposable _rightArrowPressDown;
    
    private BasicViewItemsEffects _effects;

    private BasicViewItemsEffects Effects
    {
        get
        {
            if (_effects == null)
            {
                _effects = GetComponent<BasicViewItemsEffects>();
            }

            return _effects;
        }
    }
    
    private List<InventoryItem> _items;
    private List<InventoryItem> _baseItems;

    #endregion


    public void UpdateItems(List<InventoryItem> items)
    {
        _items = items;

        if (_itemsCount != _items.Count)
        {
            _itemsCount = _items.Count;
            _itemIndex = _itemsCount / 2;
            UpdateBaseItems(_itemIndex);
        }
            
        Effects.SetName((_baseItems)[_baseItemIndex].Name);
        Effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        Effects.SetArrowsVisibility(_itemIndex - 2 > 0, _itemIndex + 2 < _items.Count - 1);

        ClearVisibleItems();
        SetVisibleItems();
    }

    public InventoryItem GetCurrentItem()
    {
        return _baseItems[_baseItemIndex];
    }
    
    private void OnLeftArrowPressed()
    {
        _baseItemIndex--;

        if (_baseItemIndex == -1)
        {
            if (_itemIndex - 2 + _baseItemIndex > -1)
            {
                _itemIndex--;
                ClearVisibleItems();
                UpdateBaseItems(_itemIndex);
                SetVisibleItems();
            }
            
            _baseItemIndex = 0;
        }
        
        Effects.SetName(_baseItems[_baseItemIndex].Name);
        Effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        Effects.SetArrowsVisibility(_itemIndex - 2 > 0, _itemIndex + 2 < _items.Count - 1);
        
    }

    private void OnRightArrowPressed()
    {
        _baseItemIndex++;
        
        if (_baseItemIndex == 5)
        {
            if (_itemIndex + 2 < _items.Count - 1)
            {
                _itemIndex++;
                ClearVisibleItems();
                UpdateBaseItems(_itemIndex);
                SetVisibleItems();
            }
            
            _baseItemIndex = 4;
        }
        
        Effects.SetName(_baseItems[_baseItemIndex].Name);
        Effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        Effects.SetArrowsVisibility(_itemIndex - 2 > 0, _itemIndex + 2 < _items.Count - 1);
    }
    
    private void UpdateBaseItems(int index)
    {
        if (_items.Count > 3)
        {
            _baseItems = new List<InventoryItem>
            {
                _items[index - 2],
                _items[index - 1],
                _items[index],
                _items[index + 1],
                _items[index + 2]
            };
        }
        else
        {
            _baseItems = _items;
        }
    }

    private void ClearVisibleItems()
    {
        for (var index = 0; index < _prefabs2D.childCount; index++)
        {
            _prefabs2D.GetChild(index).gameObject.SetActive(false);
        }
    }

    private void SetVisibleItems()
    {
        var placeholderOffset = _baseItems.Count == 1 ? 2 : 1;
        placeholderOffset = _baseItems.Count > 3 ? 0 : 1;

        var length = _baseItems.Count;
        for (var ind = 0; ind < length; ind++)
        {
            var prefab = _baseItems[ind].Prefab2D;
            var itemPosition = prefab.GetComponent<BasicViewItemPosition>();
            
            prefab.SetActive(true);
            itemPosition.SelfPosition = ind + placeholderOffset;
            itemPosition.RectTransform.localPosition = _placeholders[ind + placeholderOffset].localPosition;
        }
        
    }

    private void OnEnable()
    {
        _leftArrowPressDown = InputSystem.Instance.KeyLeftArrowPressedDown
            .Subscribe(_ => OnLeftArrowPressed())
            .AddTo(this);

        _rightArrowPressDown = InputSystem.Instance.KeyRightArrowPressedDown
            .Subscribe(_ => OnRightArrowPressed())
            .AddTo(this);
    }

    private void OnDisable()
    {
        _leftArrowPressDown.Dispose();
        _rightArrowPressDown.Dispose();

        ClearVisibleItems();
    }

}