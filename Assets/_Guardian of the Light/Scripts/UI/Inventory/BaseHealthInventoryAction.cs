using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider), typeof(Health), typeof(InventoryItemPosition))]
public abstract class BaseHealthInventoryAction : MonoBehaviour
{
    private InventoryItemPosition _itemPosition;

    private IDisposable _keyActionPressed;
    private IDisposable _keyExtraActionPressed;
    protected Health Health;

    protected Slider Slider;

    protected abstract void OnKeyActionPressed(Health playerHealth);
    protected abstract void OnKeyExtraActionPressed(Health playerHealth);

    private void OnEnable()
    {
        var playerHealth = GameManagerSystem.Instance.Player.GetComponent<Health>();

        _keyActionPressed = InputSystem.Instance.KeyActionPressed.Subscribe(_ =>
            {
                if (_itemPosition.CurrentPosition == _itemPosition.SelfPosition) OnKeyActionPressed(playerHealth);
            })
            .AddTo(this);

        _keyExtraActionPressed = InputSystem.Instance.KeyExtraActionPressed.Subscribe(_ =>
            {
                if (_itemPosition.CurrentPosition == _itemPosition.SelfPosition) OnKeyExtraActionPressed(playerHealth);
            })
            .AddTo(this);
    }

    private void OnDisable()
    {
        _keyActionPressed.Dispose();
        _keyExtraActionPressed.Dispose();
    }

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        Health = GetComponent<Health>();
        _itemPosition = GetComponent<InventoryItemPosition>();
    }
}