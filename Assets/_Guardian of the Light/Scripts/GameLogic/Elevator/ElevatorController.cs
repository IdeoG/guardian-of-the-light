using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Extensions;
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
            _input.IsAnimationPlaying = true;
            SwitchCameraToDolly();
        }

        private void OnElevatorDropAnimationPartCompleted()
        {
            _input.IsAnimationPlaying = true;
            SwitchCameraToDolly();
        }

        private void OnElevatorLiftAnimationCompleted()
        {
            _input.IsAnimationPlaying = false;
            _animator.SetBool("OpenDoor", true);
        }

        private void OnElevatorDropAnimationCompleted()
        {
            _input.IsAnimationPlaying = false;
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
                    .Where(info => info.LayerIndex == 0)
                    .Subscribe(info =>
                    {
                        var normalizedTime = info.StateInfo.normalizedTime;
                        
                        var isDoorClosingAnimationCompleted = _closingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                                                              1.0f.IsBetweenBounds(_cachedNormalizedTime0, normalizedTime);

                        var isDoorOpeningAnimationCompleted = _openingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                                                              1.0f.IsBetweenBounds(_cachedNormalizedTime0, normalizedTime);
                            
                       _cachedNormalizedTime0 = normalizedTime;
                        
                        if (isDoorClosingAnimationCompleted)
                            OnDoorClosingAnimationCompleted();
                        if (isDoorOpeningAnimationCompleted)
                            OnDoorOpeningAnimationCompleted();
                            
                    });
                
                trigger.OnStateUpdateAsObservable()
                    .Where(info => info.LayerIndex == 1)
                    .Subscribe(info =>
                    {
                        var normalizedTime = info.StateInfo.normalizedTime;
                            
                        var isElevatorLiftAnimationCompleted = 
                            _movingToLevel2.Equals(info.StateInfo.shortNameHash) &&
                            1.0f.IsBetweenBounds(_cachedNormalizedTime1, normalizedTime);
                        
                        var isElevatorDropAnimationCompleted = 
                            _movingToLevel1.Equals(info.StateInfo.shortNameHash) &&
                            1.0f.IsBetweenBounds(_cachedNormalizedTime1, normalizedTime);
                        
                        var isElevatorLiftAnimationPartCompleted = 
                            _movingToLevel2.Equals(info.StateInfo.shortNameHash) &&
                            _normalizedSwitchTime.IsBetweenBounds(_cachedNormalizedTime1, normalizedTime); 
                        
                        var isElevatorDropAnimationPartCompleted = 
                            _movingToLevel1.Equals(info.StateInfo.shortNameHash) &&
                            _normalizedSwitchTime.IsBetweenBounds(_cachedNormalizedTime1, normalizedTime);

                        _cachedNormalizedTime1 = normalizedTime;
                        
                        if (isElevatorDropAnimationCompleted)
                            OnElevatorDropAnimationCompleted();
                        if (isElevatorLiftAnimationCompleted)
                            OnElevatorLiftAnimationCompleted();
                        if (isElevatorDropAnimationPartCompleted)
                            OnElevatorDropAnimationPartCompleted();
                        if (isElevatorLiftAnimationPartCompleted)
                            OnElevatorLiftAnimationPartCompleted();
                            
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
        private float _cachedNormalizedTime0;
        private float _cachedNormalizedTime1;
        private float _cachedNormalizedTime2;
        
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