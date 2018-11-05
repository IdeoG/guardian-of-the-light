using System;
using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

[RequireComponent(typeof(BoxCollider))]
public abstract class BaseAction : MonoBehaviour
{
    private const string RequiredTag = "Player";
    private IDisposable _keyActionPressedDown;

    protected abstract void OnKeyActionPressedDown();

    private void OnTriggerEnter(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = true;
        _keyActionPressedDown = InputSystem.Instance.KeyActionPressedDown
            .Subscribe(_ => OnKeyActionPressedDown() )
            .AddTo(this);
    }

    protected void OnTriggerExit(Collider other)
    {
        InputSystem.Instance.IsPlayerInCollider = false;
        _keyActionPressedDown.Dispose();
    }
}
