using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InspectViewBehaviour : MonoBehaviour
{
    [SerializeField] private Text _itemDescription;
    [SerializeField] private Vector3 _eulerRotation;
    [SerializeField] private Transform _pivotTransform;

    [Header("Просмотр объекта")]
    [SerializeField] private float _sensetivityX;
    [SerializeField] private float _sensetivityY;
    
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
        _itemTransform.position = _pivotTransform.position;
        _itemTransform.rotation = Quaternion.Euler(_eulerRotation);
        _itemTransform.localScale = _pivotTransform.localScale;
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
        // TODO: Добавить ограничения на (min,max) scale
        _itemTransform.localScale += _sensetivityY * Vector3.one;
    }

    
    private void OnKeyDownArrowPressed()
    {
        // TODO: Добавить ограничения на (min,max) scale
        _itemTransform.localScale -= _sensetivityY * Vector3.one;
    }

    private void OnKeyLeftArrowPressed()
    {
        _itemTransform.localRotation *= Quaternion.Euler(new Vector3(0, -_sensetivityX * 1f, 0));
    }

    private void OnKeyRightArrowPressed()
    {
        _itemTransform.localRotation *= Quaternion.Euler(new Vector3(0, _sensetivityX * 1f, 0));
    }
    
    private void OnDisable()
    {
        _keyUpArrowPressed.Dispose();
        _keyDownArrowPressed.Dispose();
        _keyLeftArrowPressed.Dispose();
        _keyRightArrowPressed.Dispose();
        
        Destroy(_item);
    }
}