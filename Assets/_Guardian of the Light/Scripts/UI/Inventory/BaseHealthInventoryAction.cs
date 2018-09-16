using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider), typeof(Health))]
public abstract class BaseHealthInventoryAction : MonoBehaviour
{
    private IDisposable _keyActionPressed;
    private IDisposable _keyExtraActionPressed;
    
    protected Slider Slider;
    protected Health Health;

    protected abstract void OnKeyActionPressed(Health playerHealth);
    protected abstract void OnKeyExtraActionPressed(Health playerHealth);

    private void OnEnable()
    {
        var playerHealth = GameManagerSystem.Instance.Player.GetComponent<Health>();
        
        _keyActionPressed = InputSystem.Instance.KeyActionPressed.Subscribe(_ => OnKeyActionPressed(playerHealth))
            .AddTo(this);

        _keyExtraActionPressed = InputSystem.Instance.KeyExtraActionPressed.Subscribe(_ => OnKeyExtraActionPressed(playerHealth))
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
    }
}