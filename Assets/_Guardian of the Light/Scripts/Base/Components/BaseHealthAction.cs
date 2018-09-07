using System;
using UniRx;
using UnityEngine;

public abstract class BaseHealthAction : MonoBehaviour
{
    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressed;
    private IDisposable _keyExtraActionPressed;

    protected Animator Animator;
    
    
    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        _keyActionPressed = InputSystem.Instance.KeyActionPressed
            .Subscribe(_ => OnKeyActionPressed(other.gameObject.GetComponent<Health>()))
            .AddTo(this);
        
        _keyExtraActionPressed = InputSystem.Instance.KeyExtraActionPressed
            .Subscribe(_ => OnKeyExtraActionPressed(other.gameObject.GetComponent<Health>()))
            .AddTo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        _keyActionPressed.Dispose();
        _keyExtraActionPressed.Dispose();
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    
    protected abstract void OnKeyActionPressed(Health playerHealth);
    protected abstract void OnKeyExtraActionPressed(Health playerHealth);
}