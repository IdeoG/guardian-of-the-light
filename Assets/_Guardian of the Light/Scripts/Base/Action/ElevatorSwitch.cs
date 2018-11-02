using System;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;
using _Guardian_of_the_Light.Scripts.UI.Hint;

namespace _Guardian_of_the_Light.Scripts.Base.Action
{
    public class ElevatorSwitch : UiHint
    {
        
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
                    _hintText = _readyHintText;
                    _hintType = HintType.MultipleChoice;
                    break;
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

        protected void Awake()
        {
            base.Awake();
        }

        [SerializeField] private string _noCristalHintText;
        [SerializeField] private string _noGearHintText;
        [SerializeField] private string _readyHintText;
        
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