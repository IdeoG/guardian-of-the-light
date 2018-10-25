using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : BaseAction
{
    private Animator _animator;

    public void OnAnimationFinished()
    {
        InputSystem.Instance.IsAnimationPlaying = false;
        GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
    }
    
    protected override void OnKeyActionPressedDown()
    {
        ChangeDoorState();
    }
    
    private void ChangeDoorState()
    {
        var doorState = _animator.GetBool("isOpenDoor");
        _animator.SetBool("isOpenDoor", !doorState);

        InputSystem.Instance.IsAnimationPlaying = true;
        GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = true;

        GetComponent<BoxCollider>().enabled = false;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}