using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsBehaviour : MonoBehaviour
{
    [Header("Описание текущего предмета")] [SerializeField]
    private Text _itemDescription;

    [Header("Подсветка текущего предмета")] [SerializeField]
    private RectTransform _itemLighting;

    [Header("Пулл предметов")] [SerializeField]
    private List<GameObject> _items;

    private List<RectTransform> _rectTransforms = new List<RectTransform>();
    private List<Image> _images = new List<Image>();

    private IDisposable _leftArrowPressDown;
    private IDisposable _rightArrowPressDown;

    private int _inventoryPosition;
    private List<Item> _inventoryItems;


    private void Awake()
    {
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
        // BUG: Когда количество предметов четное - не работает на крайних предметах
        if (_inventoryPosition % 2 == 0)
        {
            if ( _inventoryPosition >= _items.Count - 4) return;
            _inventoryPosition += 2;
        }
        else
        {
            _inventoryPosition = Mathf.Clamp(_inventoryPosition - 2, 0, _items.Count);
        }

        Debug.Log(string.Format("OnLeftArrowPressedDown: _inventoryPosition = {0}, _items.Count = {1}", _inventoryPosition, _items.Count));
        SetCurrentLighting(_inventoryPosition);
        SetCurrentDescription(_inventoryItems[_inventoryPosition].Name);
    }

    private void OnRightArrowPressedDown()
    {
        // BUG: Когда количество предметов четное - не работает на крайних предметах
        if (_inventoryPosition % 2 != 0)
        {
            if (_inventoryPosition >= _items.Count - 4) return;

            _inventoryPosition = _inventoryPosition + 2;
        }
        else
        {
            if (_inventoryPosition == 0)
            {
                _inventoryPosition = 1;
            }
            else
            {
                _inventoryPosition = Mathf.Clamp(_inventoryPosition - 2, 0, _items.Count);
            }
        }

        Debug.Log(string.Format("OnRightArrowPressedDown: _inventoryPosition = {0}, _items.Count = {1}", _inventoryPosition, _items.Count));
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
        ClearImages();

        SetCurrentDescription(items[0].Name);
        SetCurrentLighting(0);

        Debug.Log(string.Format("Awake: _rectTransforms.Count = {0}", _rectTransforms.Count));
        var len = items.Count;
        for (var ind = 0; ind < len; ind++)
        {
            SetImage(ind, items[ind].Sprite, Color.white);
        }

        _inventoryItems = items;
    }

    public int GetCurrentPosition()
    {
        return _inventoryPosition;
    }
}