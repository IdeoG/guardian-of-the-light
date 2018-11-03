using UnityEngine;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;

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

        var colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            if (!collider.isTrigger) continue;
                
            collider.enabled = false;
        }
        
        OnTriggerExit(null);
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}