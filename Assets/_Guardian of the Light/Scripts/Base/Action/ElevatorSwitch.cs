using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;
using _Guardian_of_the_Light.Scripts.UI.Hint;

namespace _Guardian_of_the_Light.Scripts.Base.Action
{
    public class ElevatorSwitch : UiHint
    {
        public override void OnItemChosen(int position)
        {
            if (_elevatorLevel == 1 && position == 1)
                _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchDownHintText, null);
            else if (_elevatorLevel == 2 && position == 0)
                _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchUpHintText, null);
            else if (_elevatorLevel == 1)
                LiftElevator();
            else if (_elevatorLevel == 2)
                DropElevator();
            else
                throw new ArgumentOutOfRangeException();
        }

        private void DropElevator()
        {
            _isAnimationRunning = true;
            _input.IsAnimationPlaying = true;
            _playerControls.LockInput = true;
            
            _switch.DOLocalRotate(Vector3.left * -45, 1.5f)
                .OnComplete(() =>
                {
                    _elevatorLevel--;

                    SwitchCameraToDolly();
                    _animator.SetBool("OpenDoor", false);
                });
        }

        private void LiftElevator()
        {
            _isAnimationRunning = true;
            _input.IsAnimationPlaying = true;
            _playerControls.LockInput = true;

            _switch.DOLocalRotate(Vector3.left * 45, 1.5f)
                .OnComplete(() =>
                {
                    _elevatorLevel++;

                    SwitchCameraToDolly();
                    _animator.SetBool("OpenDoor", false);
                });
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

        private void OnCloseDoorStateExit()
        {
            _animator.SetInteger("Level", _elevatorLevel);
        }

        private void OnOpenDoorStateExit()
        {
            SwitchCameraToVirtual();

            _switch.DOLocalRotate(Vector3.zero, 1.5f)
                .OnComplete(() =>
                {
                    _isAnimationRunning = false;
                    _input.IsAnimationPlaying = false;
                    _playerControls.LockInput = false;
                });
        }

        private void OnDropElevatorCompleted()
        {
            _animator.SetBool("OpenDoor", true);
        }

        private void OnLiftElevatorCompleted()
        {
            _animator.SetBool("OpenDoor", true);
        }


        protected override void OnKeyActionPressedDown()
        {
            switch (GetMechanismState())
            {
                case ElevatorSwitchState.NoCristal:
                    _hintText = _noCristalHintText;
                    _hintType = HintType.Skip;
                    break;
                case ElevatorSwitchState.NoGear:
                    _hintText = _noGearHintText;
                    _hintType = HintType.Skip;
                    break;
                case ElevatorSwitchState.Ready:
                    _hintType = HintType.MultipleChoice;
                    _controller.OnShowHintButtonPressed(_hintType, _readyHintText, gameObject);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.OnKeyActionPressedDown();
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

        protected override void Awake()
        {
            base.Awake();
            
            _animator = transform.parent.GetComponentInParent<Animator>();
            _input = InputSystem.Instance;
            _playerControls = GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>();

            var triggers = _animator.GetBehaviours<ObservableStateMachineTrigger>();

            foreach (var trigger in triggers)
            {
                trigger.OnStateUpdateAsObservable()
                    .Subscribe(info =>
                        {
                            var isDoorClosingCompleted =
                                _closingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                                Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2 && _isAnimationRunning;

                            var isDoorOpeningCompleted =
                                _openingDoorStateHash.Equals(info.StateInfo.shortNameHash) &&
                                Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2 && _isAnimationRunning;

                            var isElevatorLiftCompleted =
                                _movingToLevel2.Equals(info.StateInfo.shortNameHash) &&
                                Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;

                            var isElevatorDropCompleted =
                                _movingToLevel1.Equals(info.StateInfo.shortNameHash) &&
                                Math.Abs(info.StateInfo.normalizedTime - 1.0f) < 1e-2;

                            if (isDoorClosingCompleted)
                                OnCloseDoorStateExit();
                            else if (isDoorOpeningCompleted)
                                OnOpenDoorStateExit();

                            if (isElevatorLiftCompleted)
                                OnLiftElevatorCompleted();
                            else if (isElevatorDropCompleted)
                                OnDropElevatorCompleted();
                        },
                        () => Debug.Log($"ElevatorDoor: OnStateUpdateAsObservable -> onCompleted"));
            }
        }

        [Header("Cameras")] 
        [SerializeField] private GameObject _trackedDollyCamera;
        [SerializeField] private GameObject _virtualCamera;

        [Header("Switch Instance")] 
        [SerializeField] private Transform _switch;

        [Header("Main hint texts")] 
        [TextArea] [SerializeField] private string _noCristalHintText;
        [TextArea] [SerializeField] private string _noGearHintText;
        [SerializeField] private List<string> _readyHintText;

        [Header("Bad action text")] 
        [SerializeField] private string _badSwitchDownHintText;
        [SerializeField] private string _badSwitchUpHintText;

        private bool _isAnimationRunning;
        private int _elevatorLevel = 1;
        private Animator _animator;
        private InputSystem _input;
        private ThirdPersonUserControl _playerControls;

        private const int CristalId = 3;
        private const int GearId = 1;
        private readonly int _movingToLevel1 = Animator.StringToHash("Moving to Level 1");
        private readonly int _movingToLevel2 = Animator.StringToHash("Moving to Level 2");
        private readonly int _openingDoorStateHash = Animator.StringToHash("Opening Door");
        private readonly int _closingDoorStateHash = Animator.StringToHash("Closing Door");
    }

    internal enum ElevatorSwitchState
    {
        NoCristal,
        NoGear,
        Ready
    }
}