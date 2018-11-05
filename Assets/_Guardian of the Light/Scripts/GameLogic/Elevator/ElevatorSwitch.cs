using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;
using _Guardian_of_the_Light.Scripts.UI.Hint;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorSwitch : UiHint, IInventoryUseAction
    {
        public override void OnItemChosen(int position)
        {
            if (_isFirstTry || !_elevatorController.IsCristalPlacedInSwitch)
            {
                if (position != 2)
                {
                    _isFirstTry = false;
                    _controller.OnShowHintButtonPressed(HintType.Empty, _noLuckText, gameObject);
                }
                else
                {
                    PlacePlayerNearCristal();
                    SwitchToInspectCamera();
                    
                    _controller.OnShowHintButtonPressed(HintType.Skip, _inspectSwitchText, gameObject);
                }
            }

            if (_elevatorController.IsCristalPlacedInSwitch && !_elevatorController.IsGearPlacedInMechanism)
            {
                if (position != 2)
                {
                    _elevatorController.PlayBrokenMechanismAnimation();
                }
                else
                {
                    PlacePlayerNearCristal();
                    SwitchToInspectCamera();
                    
                    _controller.OnShowHintButtonPressed(HintType.Skip, _inspectSwitchText, gameObject);
                }
            }

            if (_elevatorController.IsCristalPlacedInSwitch && _elevatorController.IsGearPlacedInMechanism)
            {
                if (_elevatorController.ElevatorLevel == 1 && position == 1)
                    _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchDownHintText, gameObject);
                else if (_elevatorController.ElevatorLevel == 2 && position == 0)
                    _controller.OnShowHintButtonPressed(HintType.Empty, _badSwitchUpHintText, gameObject);
                else if (_elevatorController.ElevatorLevel == 1)
                    _elevatorController.LiftElevator();
                else if (_elevatorController.ElevatorLevel == 2)
                    _elevatorController.DropElevator();
                else
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void OnSkipChosen()
        {
            SwitchToClearShotCamera();
        }

        public void OnInventoryUseAction()
        {
            _cristal.SetActive(true);
            _elevatorController.IsCristalPlacedInSwitch = true;
        }
        
        private void SwitchToInspectCamera()
        {
            _inspectCamera.SetActive(true);
            _clearShotCamera.SetActive(false);
        }

        private void SwitchToClearShotCamera()
        {
            _clearShotCamera.SetActive(true);
            _inspectCamera.SetActive(false);
        }

        private void PlacePlayerNearCristal()
        {
            GameManagerSystem.Instance.Player.transform.position = _playerTransform.position;
            GameManagerSystem.Instance.Player.transform.rotation = _playerTransform.rotation;
        }

        protected override void OnKeyActionPressedDown()
        {
            _controller.OnShowHintButtonPressed(HintType.MultipleChoice, 
                _isFirstTry ? _readyHintText.Take(2).ToList() : _readyHintText, gameObject);
        }
        
        
        protected override void Awake()
        {
            base.Awake();
            
            _elevatorController = GetComponentInParent<ElevatorController>();
        }

        
        #region SerializedFields

        [Header("Cameras")]
        [SerializeField] private GameObject _clearShotCamera;
        [SerializeField] private GameObject _inspectCamera;
        
        [Header("Main hint texts")] 
        [SerializeField] private string _noLuckText;
        [TextArea] [SerializeField] private string _inspectSwitchText;
        [SerializeField] private List<string> _readyHintText;

        [Header("Other Stuff")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private GameObject _cristal;
        [SerializeField] private string _badSwitchUpHintText;
        [SerializeField] private string _badSwitchDownHintText;
      
        #endregion
        #region PrivateVars

        private ElevatorController _elevatorController;
        private bool _isFirstTry = true;

        #endregion

    }

}