using System;
using Boo.Lang;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using _Guardian_of_the_Light.Scripts.Extensions;
using _Guardian_of_the_Light.Scripts.Systems;

public class InspectView : MonoBehaviour
{
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

    private void OnChangeItemScale(float x) // x > 0, then Increase size, else Reduce
    {
        var canReduce = Math.Abs(_minBasePositionZ) < Math.Abs(_perspectiveCameraTransform.localPosition.z)
                        && _minBasePositionZ * _perspectiveCameraTransform.localPosition.z > 0;
        var canIncrease = Math.Abs(_maxBasePositionZ) > Math.Abs(_perspectiveCameraTransform.localPosition.z)
                          && _maxBasePositionZ * _perspectiveCameraTransform.localPosition.z > 0;

        if (x > 0 ? !canReduce : !canIncrease) // TODO: Potential place of bug. Because if user will invert axis, this will no longer work
            return;

        var localPosition = _perspectiveCameraTransform.localPosition;
        localPosition.z += x * _sensitivityScale * Time.deltaTime;
        _perspectiveCameraTransform.localPosition = localPosition;
    }

    private void OnEnable()
    {
        _basePositionZ = _perspectiveCameraTransform.localPosition.z;

        _keyReduceSizePressed = InputSystem.Instance.KeyReduceSizePressed.Subscribe(_ => OnChangeItemScale(1f));
        _keyIncreaseSizePressed = InputSystem.Instance.KeyIncreaseSizePressed.Subscribe(_ => OnChangeItemScale(-1f));
        _rightStickY = InputSystem.Instance.RightStickY.Subscribe(OnChangeItemScale);

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
        _perspectiveCameraTransform.localPosition = _perspectiveCameraTransform.localPosition.With(z: _basePositionZ);

        _keyReduceSizePressed.Dispose();
        _keyIncreaseSizePressed.Dispose();

        _verticalAxis.Dispose();
        _horizontalAxis.Dispose();
        _rightStickY.Dispose();

        if (_item != null) Destroy(_item);
    }

    #region SerializeFields

    [SerializeField] private Text _itemDescription;
    [SerializeField] private LayerMask _itemLayerMask = -1;
    [SerializeField] private float _onDisableItemAnimationDuration = 0.4f;

    [Header("Animation duration")] [SerializeField]
    private float _onEnableItemAnimationDuration;

    [SerializeField] private Transform _pivotTransform;

    [Header("Inspect Item Configuration")] [SerializeField]
    private float _sensitivityX = 2f;

    [SerializeField] private float _sensitivityY = 2f;
    [SerializeField] private float _sensitivityScale = 0.02f;

    [Header("Perspective Camera")] [SerializeField]
    private Transform _perspectiveCameraTransform;

    [SerializeField] private float _minBasePositionZ = -3.7f;
    [SerializeField] private float _maxBasePositionZ = -4.4f;

    #endregion

    #region PrivateVars

    private float _basePositionZ;
    private Vector3 _itemEulerRotation = Vector3.forward;

    private GameObject _item;
    private Transform _itemTransform;

    private IDisposable _keyIncreaseSizePressed;
    private IDisposable _keyReduceSizePressed;

    private IDisposable _verticalAxis;
    private IDisposable _horizontalAxis;
    private IDisposable _rightStickY;

    #endregion
}