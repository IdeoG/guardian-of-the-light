using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BasicViewItemsEffects))]
public class BasicViewItemsController : MonoBehaviour, IBasicViewItemsController
{
    #region public_vars

    public int CurrentItemIndex;

    #endregion

    #region private_inspector_vars

    [SerializeField] private Transform _prefabs2D;
    [SerializeField] private List<RectTransform> _placeholders;

    #endregion

    #region private_vars

    private List<InventoryItem> _items;
    private BasicViewItemsEffects _effects;

    private IDisposable _leftArrowPressDown;
    private IDisposable _rightArrowPressDown;

    #endregion


    public void OnLeftArrowPressed()
    {
    }

    public void OnRightArrowPressed()
    {
    }

    public void UpdateItems(List<InventoryItem> items)
    {
        CurrentItemIndex = 0;

        _items = items;
        _effects.SetName(_items[CurrentItemIndex].Name);
        _effects.SetLightingPosition(_placeholders[CurrentItemIndex].position);
        _effects.SetArrowsVisibility(_items.Count > 6, _items.Count > 5);
        
        ClearVisibleItems();
        SetVisibleItems();
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
        var len = _items.Count > 5 ? 5 : _items.Count;
        
        for (var index = 0; index < len; index++)
        {
            var prefab = _items[index].Prefab2D;
            var itemPosition = prefab.GetComponent<BasicViewItemPosition>();

            prefab.SetActive(true);
            itemPosition.SelfPosition = index;
            itemPosition.RectTransform.localPosition = _placeholders[index].localPosition;

//            _itemsPositions.Add(prefab.GetComponent<InventoryItemPosition>());
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