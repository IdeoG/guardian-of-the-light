using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/** TODO: InspectView
 * 1. Переделать кнопки увеличения/уменьшения на UpArrow/DownArrow
 * 2. Добавить кнопки поворота по оси Z на W/S
 */
public class InspectView : MonoBehaviour
{
    private GameObject _item;
    private Transform _itemTransform;
    
    private IDisposable _keyDownArrowPressed;
    private IDisposable _keyLeftArrowPressed;
    private IDisposable _keyRightArrowPressed;
    private IDisposable _keyUpArrowPressed;
    
    [SerializeField] private Text _itemDescription;
    
    [SerializeField] private float _maxScale;
    [SerializeField] private float _minScale;
    [SerializeField] private float _onDisableItemAnimationDuration;

    [Header("Animation duration")] 
    [SerializeField] private float _onEnableItemAnimationDuration;
    [SerializeField] private Transform _pivotTransform;

    [Header("Inspect Item Configuration")] 
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;


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
        
        if (_item != null) Destroy(_item);
    }
}