using System;
using System.Collections.Generic;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;
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
            _elevatorLevel--;
            _animator.SetInteger("Level", _elevatorLevel);
        }

        private void LiftElevator()
        {
            _elevatorLevel++;
            _animator.SetInteger("Level", _elevatorLevel);
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
        }

        [TextArea] [SerializeField] private string _noCristalHintText;
        [TextArea] [SerializeField] private string _noGearHintText;
        [TextArea] [SerializeField] private string _badSwitchDownHintText;
        [TextArea] [SerializeField] private string _badSwitchUpHintText;
        [TextArea] [SerializeField] private List<string> _readyHintText;

        private int _elevatorLevel = 1;
        private Animator _animator;
        
        private const int CristalId = 3;
        private const int GearId = 1;
    }

    internal enum ElevatorSwitchState
    {
        NoCristal,
        NoGear,
        Ready
    }
}