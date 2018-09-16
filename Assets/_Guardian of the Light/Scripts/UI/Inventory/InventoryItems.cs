using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    private List<Image> _images;
    private List<InventoryItem> _inventoryItems;

    private int _inventoryPosition;
    [SerializeField] private Text _itemDescription;

    [SerializeField] private RectTransform _itemLighting;

    [Header("Items pool")]
    [SerializeField] private List<RectTransform> _itemsRectTransforms;
    [SerializeField] private Transform _prefabs2D;

    private IDisposable _leftArrowPressDown;
    private IDisposable _rightArrowPressDown;
    

    private void OnEnable()
    {
        _leftArrowPressDown = InputSystem.Instance.KeyLeftArrowPressedDown
            .Subscribe(_ => OnLeftArrowPressedDown())
            .AddTo(this);

        _rightArrowPressDown = InputSystem.Instance.KeyRightArrowPressedDown
            .Subscribe(_ => OnRightArrowPressedDown())
            .AddTo(this);
    }

    private void OnDisable()
    {
        _leftArrowPressDown.Dispose();
        _rightArrowPressDown.Dispose();
    }

    private void OnLeftArrowPressedDown()
    {
        if (_inventoryPosition % 2 == 0)
        {
            if (_inventoryPosition >= _inventoryItems.Count - 2) return;
            _inventoryPosition += 2;
        }
        else
        {
            _inventoryPosition = Mathf.Clamp(_inventoryPosition - 2, 0, _inventoryItems.Count);
        }

        SetCurrentLighting(_inventoryPosition);
        SetCurrentDescription(_inventoryItems[_inventoryPosition].Name);
    }

    private void OnRightArrowPressedDown()
    {
        if (_inventoryPosition % 2 != 0)
        {
            if (_inventoryPosition >= _inventoryItems.Count - 2) return;

            _inventoryPosition = _inventoryPosition + 2;
        }
        else
        {
            _inventoryPosition = _inventoryPosition == 0 && _inventoryItems.Count > 1
                ? 1
                : Mathf.Clamp(_inventoryPosition - 2, 0, _inventoryItems.Count);
        }

        SetCurrentLighting(_inventoryPosition);
        SetCurrentDescription(_inventoryItems[_inventoryPosition].Name);
    }

    private void SetCurrentLighting(int position)
    {
        _itemLighting.SetPositionAndRotation(_itemsRectTransforms[position].position, _itemsRectTransforms[position].rotation);
    }


    private void SetCurrentDescription(string text)
    {
        _itemDescription.text = text;
    }


    public void SetItems(List<InventoryItem> items)
    {
        ClearVisibleItems();

        SetCurrentDescription(items[_inventoryPosition].Name);
        SetCurrentLighting(_inventoryPosition);

        var len = items.Count;

        for (var ind = 0; ind < len; ind++)
        {
            var prefab = items[ind].Prefab2D;
            var rect = prefab.GetComponent<RectTransform>();
            
            prefab.SetActive(true);
            rect.localPosition = _itemsRectTransforms[ind].localPosition;
        }

        _inventoryItems = items;
    }

    private void ClearVisibleItems()
    {
        for (var index = 0; index < _prefabs2D.childCount; index++)
        {
            _prefabs2D.GetChild(index).gameObject.SetActive(false);
        }
    }

    public int GetCurrentPosition()
    {
        return _inventoryPosition;
    }
}