using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public abstract class BaseAction : MonoBehaviour
{
    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressedDown;
    protected Animator Animator;

    protected abstract void OnKeyActionPressedDown();

    private void OnTriggerEnter(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = true;
        _keyActionPressedDown = InputSystem.Instance.KeyActionPressedDown
            .Subscribe(_ => OnKeyActionPressedDown() )
            .AddTo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = false;
        _keyActionPressedDown.Dispose();
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
}