using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : BaseAction
{
    private Animator _animator;
    
    private void ChangeDoorState()
    {
        var doorState = _animator.GetBool("isOpenDoor");
        _animator.SetBool("isOpenDoor", !doorState);
    }

    protected override void OnKeyActionPressedDown()
    {
        ChangeDoorState();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}