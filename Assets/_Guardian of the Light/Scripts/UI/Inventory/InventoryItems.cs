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
    private List<BasicViewItemPosition> _itemsPositions;

    [Header("Items pool")] [SerializeField]
    private List<RectTransform> _itemsRectTransforms;

    private IDisposable _leftArrowPressDown;
    [SerializeField] private Transform _prefabs2D;
    private IDisposable _rightArrowPressDown;

    public int GetCurrentPosition()
    {
        return _inventoryPosition;
    }

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

        _itemsPositions = null;
        ClearVisibleItems();
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
        SetItemsCurrentPosition();
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
        SetItemsCurrentPosition();
    }

    private void SetCurrentLighting(int position)
    {
        _itemLighting.SetPositionAndRotation(_itemsRectTransforms[position].position,
            _itemsRectTransforms[position].rotation);
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
        _itemsPositions = new List<BasicViewItemPosition>();

        for (var ind = 0; ind < len; ind++)
        {
            var prefab = items[ind].Prefab2D;

            prefab.SetActive(true);
            prefab.GetComponent<BasicViewItemPosition>().SelfPosition = ind;
            prefab.GetComponent<RectTransform>().localPosition = _itemsRectTransforms[ind].localPosition;

            _itemsPositions.Add(prefab.GetComponent<BasicViewItemPosition>());
        }

        _inventoryItems = items;
    }

    private void ClearVisibleItems()
    {
        for (var index = 0; index < _prefabs2D.childCount; index++)
            _prefabs2D.GetChild(index).gameObject.SetActive(false);
    }

    private void SetItemsCurrentPosition()
    {
        foreach (var _itemPosition in _itemsPositions) _itemPosition.CurrentPosition = _inventoryPosition;
    }
}