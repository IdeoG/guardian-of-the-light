using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorDoor : BaseAction
    {
        protected override void OnKeyActionPressedDown()
        {
            InputSystem.Instance.IsAnimationPlaying = true;
            GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = true;
            
            _animator.SetBool("OpenDoor", !_animator.GetBool("OpenDoor"));
        }

        private void Awake()
        {
            _animator = transform.parent.GetComponentInParent<Animator>();
            var triggers = _animator.GetBehaviours<ObservableStateMachineTrigger>();

            foreach (var trigger in triggers)
            {
                trigger.OnStateUpdateAsObservable()
                    .Subscribe(info =>
                        {
                            if (info.LayerIndex != 0) 
                                return;
                            
                            if (_closingDoorStateHash.Equals(info.StateInfo.shortNameHash) && Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2)
                                OnDoorStateExit();
                            else if (_openingDoorStateHash.Equals(info.StateInfo.shortNameHash) && Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2)
                                OnDoorStateExit();
                        },
                        () => Debug.Log($"ElevatorDoor: OnStateUpdateAsObservable -> onCompleted"));
            }
        }

        private void OnDoorStateExit()
        {
            InputSystem.Instance.IsAnimationPlaying = false;
            GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
        }

        private Animator _animator;
        private readonly int _openingDoorStateHash = Animator.StringToHash("Opening Door");
        private readonly int _closingDoorStateHash = Animator.StringToHash("Closing Door");
    }
}