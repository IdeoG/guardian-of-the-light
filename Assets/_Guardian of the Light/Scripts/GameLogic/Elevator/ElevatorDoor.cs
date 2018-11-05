using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorDoor : BaseAction
    {
        private Animator _animator;

        protected override void OnKeyActionPressedDown()
        {
            InputSystem.Instance.IsAnimationPlaying = true;

            _animator.SetBool("OpenDoor", !_animator.GetBool("OpenDoor"));
        }

        private void Awake()
        {
            _animator = GetComponentInParent<Animator>();
        }
    }
}