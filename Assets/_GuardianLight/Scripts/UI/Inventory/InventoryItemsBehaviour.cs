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

    private IDisposable _leftArrowPress;
    private IDisposable _rightArrowPress;

    private int _inventoryPosition;

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
        _leftArrowPress = InputSystem.Instance.KeyLeftArrowPressed
            .Subscribe(_ =>
            {
                Debug.Log(string.Format("LeftArrowPressed"));
                OnLeftArrowPressed();
            })
            .AddTo(this);

        _rightArrowPress = InputSystem.Instance.KeyRightArrowPressed
            .Subscribe(_ => OnRightArrowPressed())
            .AddTo(this);
    }

    private void OnDisable()
    {
        _leftArrowPress.Dispose();
        _rightArrowPress.Dispose();
    }

    private void OnLeftArrowPressed()
    {
        if (_inventoryPosition % 2 == 0)
        {
            if (_inventoryPosition >= _items.Count - 2) return;
            _inventoryPosition += 2;
        }
        else
        {
            _inventoryPosition = Mathf.Clamp(_inventoryPosition - 2, 0, _items.Count);
        }

        SetCurrentLighting(_inventoryPosition);
    }

    private void OnRightArrowPressed()
    {
        if (_inventoryPosition % 2 != 0)
        {
            if (_inventoryPosition >= _items.Count - 1) return;

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

        SetCurrentLighting(_inventoryPosition);
    }

    private void SetCurrentLighting(int position)
    {
        _itemLighting.SetPositionAndRotation(_rectTransforms[position].position, _rectTransforms[position].rotation);
    }

    public void SetImage(int position, Sprite sprite, Color color)
    {
        _images[position].sprite = sprite;
        _images[position].color = color;
    }

    public void SetCurrentDescription(string text)
    {
        _itemDescription.text = text;
    }

    public void ClearImages()
    {
        foreach (var image in _images)
        {
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
        }
    }
}