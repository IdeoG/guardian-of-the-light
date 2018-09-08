﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    private List<Image> _images;
    private List<Item> _inventoryItems;

    private int _inventoryPosition;
    [SerializeField] private Text _itemDescription;

    [SerializeField] private RectTransform _itemLighting;

    [Header("Items pool")] [SerializeField]
    private List<GameObject> _items;

    private IDisposable _leftArrowPressDown;

    private List<RectTransform> _rectTransforms;
    private IDisposable _rightArrowPressDown;


    private void FetchItems()
    {
        _rectTransforms = new List<RectTransform>();
        _images = new List<Image>();

        foreach (var item in _items)
        {
            _rectTransforms.Add(item.GetComponent<RectTransform>());
            _images.Add(item.GetComponent<Image>());
        }
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
            _inventoryPosition = _inventoryPosition == 0
                ? 1
                : Mathf.Clamp(_inventoryPosition - 2, 0, _inventoryItems.Count);
        }

        SetCurrentLighting(_inventoryPosition);
        SetCurrentDescription(_inventoryItems[_inventoryPosition].Name);
    }

    private void SetCurrentLighting(int position)
    {
        _itemLighting.SetPositionAndRotation(_rectTransforms[position].position, _rectTransforms[position].rotation);
    }

    private void SetImage(int position, Sprite sprite, Color color)
    {
        _images[position].sprite = sprite;
        _images[position].color = color;
    }

    private void SetCurrentDescription(string text)
    {
        _itemDescription.text = text;
    }

    private void ClearImages()
    {
        foreach (var image in _images)
        {
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
        }
    }

    public void SetItems(List<Item> items)
    {
        if (_images == null) FetchItems();

        ClearImages();

        SetCurrentDescription(items[_inventoryPosition].Name);
        SetCurrentLighting(_inventoryPosition);

        var len = items.Count;

        for (var ind = 0; ind < len; ind++) SetImage(ind, items[ind].Sprite, Color.white);

        _inventoryItems = items;
    }

    public int GetCurrentPosition()
    {
        return _inventoryPosition;
    }
}