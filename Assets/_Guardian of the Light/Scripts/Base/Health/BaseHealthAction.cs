using System;
using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

[RequireComponent(typeof(Health), typeof(BoxCollider))]
public abstract class BaseHealthAction : MonoBehaviour
{
    private IDisposable _keyActionPressed;
    private IDisposable _keyExtraActionPressed;
    protected Animator Animator;
    protected Health Health;


    protected abstract void OnKeyActionPressed(Health playerHealth);
    protected abstract void OnKeyExtraActionPressed(Health playerHealth);

    private void OnTriggerEnter(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = true;
        var playerHealth = GameManagerSystem.Instance.Player.GetComponent<Health>();

        _keyActionPressed = InputSystem.Instance.KeyActionPressed
            .Subscribe(_ => OnKeyActionPressed(playerHealth))
            .AddTo(this);

        _keyExtraActionPressed = InputSystem.Instance.KeyExtraActionPressed
            .Subscribe(_ => OnKeyExtraActionPressed(playerHealth) )
            .AddTo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = false;
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