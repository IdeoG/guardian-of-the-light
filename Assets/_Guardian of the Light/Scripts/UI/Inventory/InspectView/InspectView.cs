using System;
using Boo.Lang;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using _Guardian_of_the_Light.Scripts.Systems;

public class InspectView : MonoBehaviour
{
    private GameObject _item;

    [SerializeField] private Text _itemDescription;
    [SerializeField] private LayerMask _itemLayerMask = -1;
    [SerializeField] private float _onDisableItemAnimationDuration = 0.4f;

    [Header("Animation duration")] 
    [SerializeField] private float _onEnableItemAnimationDuration;
    [SerializeField] private Transform _pivotTransform;

    [Header("Inspect Item Configuration")] 
    [SerializeField] private float _sensitivityX = 2f;
    [SerializeField] private float _sensitivityY = 2f;
    [SerializeField] private float _sensitivityScale = 0.02f;
    
    [Header("Perspective Camera")]
    [SerializeField] private Transform _perspectiveCameraTransform;
    [SerializeField] private float _basePositionZ = -3.5f;
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxScale = 2f;

    private Transform _itemTransform;
    private IDisposable _keyDownArrowPressed;
    private IDisposable _keyIncreaseSizePressed;
    private IDisposable _keyLeftArrowPressed;
    private IDisposable _keyReduceSizePressed;
    private IDisposable _keyRightArrowPressed;
    private IDisposable _keyUpArrowPressed;
    
    private IDisposable _verticalAxis;
    private IDisposable _horizontalAxis;

    private Vector3 _itemEulerRotation = Vector3.forward;
    
    public void PlayOnEnableAnimation()
    {
        _item.transform.DOLocalMove(Vector3.zero, _onEnableItemAnimationDuration);
        _item.transform.DOScale(1, _onEnableItemAnimationDuration);
        _item.transform.DOLocalRotate(
            new Vector3(_itemEulerRotation.x, 360f + _itemEulerRotation.y, _itemEulerRotation.z),
            _onEnableItemAnimationDuration,
            RotateMode.FastBeyond360);
    }

    public void PlayOnDisableAnimation()
    {
        _item.transform.DOLocalMove(new Vector3(0, -0.5f, 0), _onDisableItemAnimationDuration);
        _item.transform.DOScale(0, _onDisableItemAnimationDuration)
            .OnComplete(() => Destroy(_item));
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        _itemDescription.text = inventoryItem.Description;
        
        _item = Instantiate(inventoryItem.Prefab3D);
        _item.layer = Mathf.RoundToInt(Mathf.Log(_itemLayerMask.value, 2));

        _itemTransform = _item.transform;
        _itemEulerRotation = _itemTransform.localEulerAngles;
        _itemTransform.parent = _pivotTransform;

        _itemTransform.localPosition = new Vector3(0, -0.5f, 0);
        _itemTransform.localRotation = Quaternion.Euler(0, 180f, 0);
        _itemTransform.localScale = Vector3.zero;
    }

    private void OnKeyIncreaseSizePressed()
    {
        if (_perspectiveCameraTransform.localPosition.z < _maxScale * _basePositionZ) return;

        var localPosition = _perspectiveCameraTransform.localPosition;
        localPosition.z -= _sensitivityScale * Time.deltaTime;
        _perspectiveCameraTransform.localPosition = localPosition;
    }

    private void OnKeyReduceSizePressed()
    {
        if (_perspectiveCameraTransform.localPosition.z > _minScale * _basePositionZ) return;

        var localPosition = _perspectiveCameraTransform.localPosition;
        localPosition.z += _sensitivityScale * Time.deltaTime;
        _perspectiveCameraTransform.localPosition = localPosition;
    }
    private void OnEnable()
    {
        _perspectiveCameraTransform.localPosition = new Vector3(0, 0, _basePositionZ);
        
        _keyReduceSizePressed = InputSystem.Instance.KeyReduceSizePressed.Subscribe(_ => OnKeyReduceSizePressed()).AddTo(this);
        _keyIncreaseSizePressed = InputSystem.Instance.KeyIncreaseSizePressed.Subscribe(_ => OnKeyIncreaseSizePressed()).AddTo(this);
        
        _verticalAxis = InputSystem.Instance.VerticalAxis.Subscribe(OnVerticalAxisChanged);
        _horizontalAxis = InputSystem.Instance.HorizontalAxis.Subscribe(OnHorizontalAxisChanged);
    }

    private void OnHorizontalAxisChanged(float x)
    {
        _itemTransform.Rotate(new Vector3(0, -x, 0), _sensitivityX * Time.deltaTime, Space.World);
    }

    private void OnVerticalAxisChanged(float y)
    {
        _itemTransform.Rotate(new Vector3(y, 0, 0), _sensitivityY * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        _keyReduceSizePressed.Dispose();
        _keyIncreaseSizePressed.Dispose();
        
        _verticalAxis.Dispose();
        _horizontalAxis.Dispose();

        if (_item != null) Destroy(_item);
        
    }
}