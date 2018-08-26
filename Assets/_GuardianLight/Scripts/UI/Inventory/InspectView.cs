using System;
using DG.Tweening;
using DG.Tweening.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InspectView : MonoBehaviour
{
    [SerializeField] private Text _itemDescription;
    [SerializeField] private Transform _pivotTransform;

    [Header("Просмотр объекта")]
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    
    private GameObject _item;
    private Transform _itemTransform;

    private IDisposable _keyLeftArrowPressed;
    private IDisposable _keyRightArrowPressed;
    private IDisposable _keyUpArrowPressed;
    private IDisposable _keyDownArrowPressed;
    
    
    public void SetItem(Item item)
    {
        _itemDescription.text = item.Description;

        _item = Instantiate(item.Prefab);
        _item.layer = LayerMask.NameToLayer("UI");


        _itemTransform = _item.transform;
        _itemTransform.parent = _pivotTransform;
        
        _itemTransform.localPosition = Vector3.zero;
        _itemTransform.localRotation = Quaternion.Euler(0, 180f, 0);
        _itemTransform.localScale = Vector3.zero;

        _item.transform.DOScale(1, 2);
        _item.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3, RotateMode.FastBeyond360);
    }

    private void OnEnable()
    {
        _keyUpArrowPressed = InputSystem.Instance.KeyUpArrowPressed
            .Subscribe(_ => OnKeyUpArrowPressed())
            .AddTo(this);
        
        _keyDownArrowPressed = InputSystem.Instance.KeyDownArrowPressed
            .Subscribe(_ => OnKeyDownArrowPressed())
            .AddTo(this);
        
        _keyLeftArrowPressed = InputSystem.Instance.KeyLeftArrowPressed
            .Subscribe(_ => OnKeyLeftArrowPressed())
            .AddTo(this);
        
        _keyRightArrowPressed = InputSystem.Instance.KeyRightArrowPressed
            .Subscribe(_ => OnKeyRightArrowPressed())
            .AddTo(this);
    }

    private void OnKeyUpArrowPressed()
    {
        if (_itemTransform.localScale.x > _maxScale) return;
        
        _itemTransform.localScale += _sensitivityY * Vector3.one;
    }

    
    private void OnKeyDownArrowPressed()
    {
        if (_itemTransform.localScale.x < _minScale) return;
        
        _itemTransform.localScale -= _sensitivityY * Vector3.one;
    }

    private void OnKeyLeftArrowPressed()
    {
        _itemTransform.localRotation *= Quaternion.Euler(new Vector3(0, -_sensitivityX * 1f, 0));
    }

    private void OnKeyRightArrowPressed()
    {
        _itemTransform.localRotation *= Quaternion.Euler(new Vector3(0, _sensitivityX * 1f, 0));
    }
    
    private void OnDisable()
    {
        _keyUpArrowPressed.Dispose();
        _keyDownArrowPressed.Dispose();
        _keyLeftArrowPressed.Dispose();
        _keyRightArrowPressed.Dispose();

        Destroy(_item);
//        _item.transform.DOScale(0, 1);
//        _item.transform.DOLocalRotate(new Vector3(0, -360f, 0), 2, RotateMode.FastBeyond360).OnComplete(() => Destroy(_item) );
    }

}