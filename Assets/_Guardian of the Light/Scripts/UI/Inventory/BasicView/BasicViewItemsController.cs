using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BasicViewItemsEffects))]
public class BasicViewItemsController : MonoBehaviour, IBasicViewItemsController
{
    #region public_vars


    #endregion

    #region private_inspector_vars

    [SerializeField] private Transform _prefabs2D;
    [SerializeField] private List<RectTransform> _placeholders;

    #endregion

    #region private_vars
        
    private int _itemIndex;
    private int _baseItemIndex;
    
    private IDisposable _leftArrowPressDown;
    private IDisposable _rightArrowPressDown;
    
    private BasicViewItemsEffects _effects;
    
    private List<InventoryItem> _items;
    private List<InventoryItem> _baseItems;

    #endregion


    public void OnLeftArrowPressed()
    {
        _baseItemIndex--;

        if (_baseItemIndex == -1)
        {
            if (_itemIndex - 2 + _baseItemIndex > -1)
            {
                _itemIndex--;
                ClearVisibleItems();
                SetVisibleItems(_itemIndex);
            }
            
            _baseItemIndex = 0;
        }
        
        _effects.SetName(_baseItems[_baseItemIndex].Name);
        _effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        _effects.SetArrowsVisibility(_itemIndex - 2 + _baseItemIndex > 0, _itemIndex + 2 < _items.Count - 1);
        
    }

    public void OnRightArrowPressed()
    {
        _baseItemIndex++;
        
        if (_baseItemIndex == 5)
        {
            if (_itemIndex + 2 < _items.Count - 1)
            {
                _itemIndex++;
                ClearVisibleItems();
                SetVisibleItems(_itemIndex);
            }
            
            _baseItemIndex = 4;
        }
        
        _effects.SetName(_baseItems[_baseItemIndex].Name);
        _effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        _effects.SetArrowsVisibility(_itemIndex - 2 + _baseItemIndex > 0, _itemIndex + 2 < _items.Count - 1);
    }

    public void UpdateItems(List<InventoryItem> items)
    {
        _items = items;

        _baseItemIndex = 2;
        _itemIndex = _items.Count / 2;
            
        _effects.SetName(_items[_baseItemIndex].Name);
        _effects.SetLightingPosition(_placeholders[_baseItemIndex].position);
        _effects.SetArrowsVisibility(_items.Count > 6, _items.Count > 5);

        ClearVisibleItems();
        SetVisibleItems(_itemIndex);
    }

    private void ClearVisibleItems()
    {
        for (var index = 0; index < _prefabs2D.childCount; index++)
        {
            _prefabs2D.GetChild(index).gameObject.SetActive(false);
        }
    }

    private void SetVisibleItems(int index)
    {
        var placeholderOffset = 0;
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
            placeholderOffset = _items.Count == 1 ? 2 : 1;
            _baseItems = _items;
        }

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

    private void Awake()
    {
        _effects = GetComponent<BasicViewItemsEffects>();
    }
}