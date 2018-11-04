using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorController : MonoBehaviour
    {
        private bool _isElevatorRunning;

        #region public

        

        #endregion
        #region serializable_fields

        [Header("Cameras")] 
        [SerializeField] private GameObject _trackedDollyCamera;
        [SerializeField] private GameObject _virtualCamera;
        [SerializeField] private float _normalizedSwitchTime = 0.75f;

        [Header("Switch Instance")] 
        [SerializeField] private Transform _switch;

        #endregion
        #region private_vars

        private Animator _animator;
        
        private const int CristalId = 3;
        private const int GearId = 1;
        private readonly int _movingToLevel1 = Animator.StringToHash("Moving to Level 1");
        private readonly int _movingToLevel2 = Animator.StringToHash("Moving to Level 2");
        private readonly int _openingDoorStateHash = Animator.StringToHash("Opening Door");
        private readonly int _closingDoorStateHash = Animator.StringToHash("Closing Door");
        
        #endregion
        
        
        private void OnDoorOpeningAnimationCompleted() {
            Debug.Log($"{MTime.Now} ElevatorController: OnDoorOpened -> content");
            
        }

        private void OnDoorClosingAnimationCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnDoorClosed -> content");
        }

        private void OnElevatorLiftAnimationPartCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnElevatorLiftAnimationPartCompleted -> content");
        }

        private void OnElevatorDropAnimationPartCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnElevatorDropAnimationPartCompleted -> content");
        }

        private void OnElevatorLiftAnimationCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnElevatorLiftAnimationCompleted -> content");
        }

        private void OnElevatorDropAnimationCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnElevatorDropAnimationCompleted -> content");
        }

        private void OnBrokenMechanismAnimationCompleted()
        {
            Debug.Log($"{MTime.Now} ElevatorController: OnBrokenMechanismAnimationCompleted -> content");
        }
        
        private ElevatorSwitchState GetMechanismState()
        {
            var inventory = InventorySystem.Instance;
            var isCristalTook = inventory.GetItemById(CristalId).IsTook;
            var isGearTook = inventory.GetItemById(GearId).IsTook;

            if (!isCristalTook) return ElevatorSwitchState.NoCristal;
            if (!isGearTook) return ElevatorSwitchState.NoGear;

            return ElevatorSwitchState.Ready;
        }
      
        private void SwitchCameraToDolly()
        {
            _trackedDollyCamera.SetActive(true);
            _virtualCamera.SetActive(false);
        }

        private void SwitchCameraToVirtual()
        {
            _virtualCamera.SetActive(true);
            _trackedDollyCamera.SetActive(false);
        }

        private void InitializeAnimationTriggers()
        {
            var triggers = _animator.GetBehaviours<ObservableStateMachineTrigger>();

            foreach (var trigger in triggers)
            {
                trigger.OnStateUpdateAsObservable()
                    .Subscribe(info =>
                    {
                        var isDoorClosingAnimationCompleted = _closingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;

                        var isDoorOpeningAnimationCompleted = _openingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;
                            
                        var isElevatorLiftAnimationCompleted = _movingToLevel2.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;

                        var isElevatorDropAnimationCompleted = _movingToLevel1.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;

                        var isElevatorLiftAnimationPartCompleted = _movingToLevel2.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - _normalizedSwitchTime) < 1e-2;
                            
                        var isElevatorDropAnimationPartCompleted = _movingToLevel1.Equals(info.StateInfo.shortNameHash) &&
                            Math.Abs(info.StateInfo.normalizedTime - _normalizedSwitchTime) < 1e-2;
                        
                        switch (info.LayerIndex)
                        {
                            case 0:
                                if (isDoorClosingAnimationCompleted)
                                    OnDoorClosingAnimationCompleted();
                                if (isDoorOpeningAnimationCompleted)
                                    OnDoorOpeningAnimationCompleted();
                                break;
                            case 1:
                                if (isElevatorDropAnimationCompleted)
                                    OnElevatorDropAnimationCompleted();
                                if (isElevatorLiftAnimationCompleted)
                                    OnElevatorLiftAnimationCompleted();
                                if (isElevatorDropAnimationPartCompleted)
                                    OnElevatorDropAnimationPartCompleted();
                                if (isElevatorLiftAnimationPartCompleted)
                                    OnElevatorLiftAnimationPartCompleted();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                            
                    });
            }
        }

        private void Awake()
        {
            _animator = GetComponentInParent<Animator>();
            
            InitializeAnimationTriggers();
        }
    }
    
    internal enum ElevatorSwitchState
    {
        NoCristal,
        NoGear,
        Ready
    }
}