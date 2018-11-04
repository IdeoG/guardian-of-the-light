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

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorSwitch : UiHint
    {
        public override void OnItemChosen(int position)
        {
            if (_elevatorController.ElevatorLevel == 1 && position == 1)
                _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchDownHintText, null);
            else if (_elevatorController.ElevatorLevel == 2 && position == 0)
                _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchUpHintText, null);
            else if (_elevatorController.ElevatorLevel == 1)
                _elevatorController.LiftElevator();
            else if (_elevatorController.ElevatorLevel == 2)
                _elevatorController.DropElevator();
            else
                throw new ArgumentOutOfRangeException();
        }

        protected override void OnKeyActionPressedDown()
        {
            switch (GetElevatorSwitchState())
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
        
        
        private ElevatorSwitchState GetElevatorSwitchState()
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
            
            _elevatorController = GetComponentInParent<ElevatorController>();
        }


        [Header("Main hint texts")] 
        [TextArea] [SerializeField] private string _noCristalHintText;
        [TextArea] [SerializeField] private string _noGearHintText;
        [SerializeField] private List<string> _readyHintText;

        [Header("Bad action text")] 
        [SerializeField] private string _badSwitchDownHintText;
        [SerializeField] private string _badSwitchUpHintText;

        private ElevatorController _elevatorController;

        private const int CristalId = 3;
        private const int GearId = 1;
    }

}