using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public abstract class BaseAction : MonoBehaviour
{
    protected Animator Animator;

    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressedDown;

    protected abstract void OnKeyActionPressedDown();

    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.tag.Equals(RequiredTag);

        if (!isPlayer) return;

        _keyActionPressedDown = InputSystem.Instance.KeyActionPressedDown
            .Subscribe(_ =>
            {
                if (!InputSystem.Instance.IsInInventory) OnKeyActionPressedDown();
            })
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
}