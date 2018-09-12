using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InspectView : MonoBehaviour
{
    private GameObject _item;
    private Transform _itemTransform;
    
    private IDisposable _keyUpArrowPressed;
    private IDisposable _keyDownArrowPressed;
    private IDisposable _keyLeftArrowPressed;
    private IDisposable _keyRightArrowPressed;
    private IDisposable _keyReduceSizePressed;
    private IDisposable _keyIncreaseSizePressed;
    
    [SerializeField] private Text _itemDescription;
    
    [SerializeField] private float _maxScale = 2f;
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _onDisableItemAnimationDuration = 0.4f;

    [Header("Animation duration")] 
    [SerializeField] private float _onEnableItemAnimationDuration;
    [SerializeField] private Transform _pivotTransform;

    [Header("Inspect Item Configuration")] 
    [SerializeField] private float _sensitivityX = 2f;
    [SerializeField] private float _sensitivityY = 2f;
    [SerializeField] private float _sensitivityScale = 0.02f;


    public void PlayOnEnableAnimation()
    {
        _item.transform.DOLocalMove(Vector3.zero, _onEnableItemAnimationDuration);
        _item.transform.DOScale(1, _onEnableItemAnimationDuration);
        _item.transform.DOLocalRotate(new Vector3(0, 360f, 0), _onEnableItemAnimationDuration,
            RotateMode.FastBeyond360);
    }

    public void PlayOnDisableAnimation()
    {
        _item.transform.DOLocalMove(new Vector3(0, -0.5f, 0), _onDisableItemAnimationDuration);
        _item.transform.DOScale(0, _onDisableItemAnimationDuration);
        _item.transform.DOLocalRotate(new Vector3(0, 180f, 0), _onDisableItemAnimationDuration,
                RotateMode.FastBeyond360)
            .OnComplete(() => Destroy(_item));
    }

    public void SetItem(Item item)
    {
        _itemDescription.text = item.Description;

        _item = Instantiate(item.Prefab);
        _item.layer = LayerMask.NameToLayer("UI");

        _itemTransform = _item.transform;
        _itemTransform.parent = _pivotTransform;

        _itemTransform.localPosition = new Vector3(0, -0.5f, 0);
        _itemTransform.localRotation = Quaternion.Euler(0, 180f, 0);
        _itemTransform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _keyUpArrowPressed = InputSystem.Instance.KeyUpArrowPressed.Subscribe(_ => OnKeyUpArrowPressed()).AddTo(this);
        _keyDownArrowPressed = InputSystem.Instance.KeyDownArrowPressed.Subscribe(_ => OnKeyDownArrowPressed()).AddTo(this);
        _keyLeftArrowPressed = InputSystem.Instance.KeyLeftArrowPressed.Subscribe(_ => OnKeyLeftArrowPressed()).AddTo(this);
        _keyRightArrowPressed = InputSystem.Instance.KeyRightArrowPressed.Subscribe(_ => OnKeyRightArrowPressed()).AddTo(this);
        _keyReduceSizePressed = InputSystem.Instance.KeyReduceSizePressed.Subscribe(_ => OnKeyReduceSizePressed()).AddTo(this);
        _keyIncreaseSizePressed = InputSystem.Instance.KeyIncreaseSizePressed.Subscribe(_ => OnKeyIncreaseSizePressed()).AddTo(this);
    }

    private void OnKeyIncreaseSizePressed()
    {
        if (_itemTransform.localScale.x > _maxScale) return;

        _itemTransform.localScale += _sensitivityScale * Vector3.one * Time.deltaTime;
    }

    private void OnKeyReduceSizePressed()
    {
        if (_itemTransform.localScale.x < _minScale) return;

        _itemTransform.localScale -= _sensitivityScale * Vector3.one * Time.deltaTime;
    }

    private void OnKeyUpArrowPressed()
    {
        //_itemTransform.rotation *= Quaternion.Euler(new Vector3( _sensitivityY * 1f, 0, 0));
        //TODO: умножили значение _sensitivityY на Time.deltaTime 
        _itemTransform.Rotate(new Vector3(1f, 0, 0), _sensitivityY * Time.deltaTime, Space.World);
    }

    private void OnKeyDownArrowPressed()
    {
        _itemTransform.Rotate(new Vector3(1f, 0, 0), -_sensitivityY * Time.deltaTime, Space.World);
    }

    private void OnKeyLeftArrowPressed()
    {
        _itemTransform.Rotate(new Vector3(0, 1f, 0), _sensitivityX * Time.deltaTime, Space.World);
    }

    private void OnKeyRightArrowPressed()
    {
        _itemTransform.Rotate(new Vector3(0, 1f, 0), -_sensitivityX * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        _keyUpArrowPressed.Dispose();
        _keyDownArrowPressed.Dispose();
        _keyLeftArrowPressed.Dispose();
        _keyRightArrowPressed.Dispose();
        _keyReduceSizePressed.Dispose();
        _keyIncreaseSizePressed.Dispose();
     
        if (_item != null) Destroy(_item);
    }
}