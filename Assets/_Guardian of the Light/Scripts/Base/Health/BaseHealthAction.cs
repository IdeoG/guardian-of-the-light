using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Health), typeof(BoxCollider))]
public abstract class BaseHealthAction : MonoBehaviour
{
    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressed;
    private IDisposable _keyExtraActionPressed;

    protected Animator Animator;
    protected Health Health;
    
    protected abstract void OnKeyActionPressed(Health playerHealth);
    protected abstract void OnKeyExtraActionPressed(Health playerHealth);
    
    private void OnTriggerEnter(Collider other)
    {    
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        var playerHealth = GameManagerSystem.Instance.Player.GetComponent<Health>();

        _keyActionPressed = InputSystem.Instance.KeyActionPressed
            .Subscribe(_ => OnKeyActionPressed(playerHealth))
            .AddTo(this);
        
        _keyExtraActionPressed = InputSystem.Instance.KeyExtraActionPressed
            .Subscribe(_ => OnKeyExtraActionPressed(playerHealth))
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
        Health = GetComponent<Health>();

        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    
}