using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public abstract class BaseActionBehaviour : MonoBehaviour
{
    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressedDown;
    protected Animator Animator;

    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        _keyActionPressedDown = InputSystem.Instance.KeyActionPressedDown
            .Subscribe(_ => OnKeyActionPressedDown())
            .AddTo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        _keyActionPressedDown.Dispose();
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    protected abstract void OnKeyActionPressedDown();
}