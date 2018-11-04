using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorController : MonoBehaviour
    {
        
        public void LiftElevator()
        {
            _isElevatorRunning = true;
            _input.IsAnimationPlaying = true;

            _switch.DOLocalRotate(Vector3.left * 45, 1.5f)
                .OnComplete(() =>
                {
                    ElevatorLevel++;
                    _input.IsAnimationPlaying = false;
                    _animator.SetBool("OpenDoor", false);
                });
        }
        
        public void DropElevator()
        {
            _isElevatorRunning = true;
            _input.IsAnimationPlaying = true;
            
            _switch.DOLocalRotate(Vector3.left * -45, 1.5f)
                .OnComplete(() =>
                {
                    ElevatorLevel--;
                    _input.IsAnimationPlaying = false;
                    _animator.SetBool("OpenDoor", false);
                });
        }
        
        private void OnDoorOpeningAnimationCompleted()
        {
            if (!_isElevatorRunning)
                _input.IsAnimationPlaying = false;
            else
            {
                SwitchCameraToVirtual();

                _switch.DOLocalRotate(Vector3.zero, 1.5f)
                    .OnComplete(() =>
                    {
                        _isElevatorRunning = false;
                        _input.IsAnimationPlaying = false;
                    });
            }
        }

        private void OnDoorClosingAnimationCompleted()
        {
            if (!_isElevatorRunning)
                _input.IsAnimationPlaying = false;
            else
                _animator.SetInteger("Level", ElevatorLevel);
        }

        private void OnElevatorLiftAnimationPartCompleted()
        {
            SwitchCameraToDolly();
            _input.IsAnimationPlaying = true;
        }

        private void OnElevatorDropAnimationPartCompleted()
        {
            SwitchCameraToDolly();
            _input.IsAnimationPlaying = true;
        }

        private void OnElevatorLiftAnimationCompleted()
        {
            _animator.SetBool("OpenDoor", true);
        }

        private void OnElevatorDropAnimationCompleted()
        {
            _animator.SetBool("OpenDoor", true);
        }

        private void OnBrokenMechanismAnimationCompleted()
        {
            
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
            _input = InputSystem.Instance;
            
            InitializeAnimationTriggers();
        }
        
        

        #region public

        public int ElevatorLevel = 1;

        #endregion
        #region serializable_fields

        [Header("Cameras")] 
        [SerializeField] private GameObject _trackedDollyCamera;
        [SerializeField] private GameObject _virtualCamera;
        [SerializeField] private float _normalizedSwitchTime = 0.75f;

        [Header("GameObject Instances")] 
        [SerializeField] private Transform _switch;
        [SerializeField] private GameObject _gear;
        [SerializeField] private GameObject _cristal;

        #endregion
        #region private_vars

        private Animator _animator;
        private InputSystem _input;
        private bool _isElevatorRunning;
        
        private readonly int _movingToLevel1 = Animator.StringToHash("Moving to Level 1");
        private readonly int _movingToLevel2 = Animator.StringToHash("Moving to Level 2");
        private readonly int _openingDoorStateHash = Animator.StringToHash("Opening Door");
        private readonly int _closingDoorStateHash = Animator.StringToHash("Closing Door");
        
        #endregion

    }
    
    internal enum ElevatorSwitchState
    {
        NoCristal,
        NoGear,
        Ready
    }
}