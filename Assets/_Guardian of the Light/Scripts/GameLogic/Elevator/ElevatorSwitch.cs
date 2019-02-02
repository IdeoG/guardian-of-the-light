using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
                    _switch.DOLocalRotate(Vector3.left * (position == 0 ? 45 : -45), 1.5f)
                        .OnComplete(() => 
                            _switch.DOLocalRotate(Vector3.zero, 1.5f)
                                   .OnComplete(() => _controller.OnShowHintButtonPressed(HintType.Empty, _noLuckText, gameObject)));
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
                switch (position)
                {
                    case 0:
                        _elevatorController.PlayBrokenMechanismSwitchUpAnimation();
                        break;
                    case 1:
                        _elevatorController.PlayBrokenMechanismSwitchDownAnimation();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
            _isSwitchInspected = true;
            SwitchToClearShotCamera();
        }

        public bool OnInventoryUseAction(int itemId)
        {
            if (ElevatorCristalId != itemId || !_isSwitchInspected) 
                return false;
            
            _cristal.SetActive(true);
            _elevatorController.IsCristalPlacedInSwitch = true;
            return true;
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
            GGameManager.Instance.Player.transform.position = _playerTransform.position;
            GGameManager.Instance.Player.transform.rotation = _playerTransform.rotation;
        }

        protected override void OnKeyActionPressedDown()
        {
            _controller.OnShowHintButtonPressed(HintType.MultipleChoice, 
                _isFirstTry || IsCristalPlacedInSwitch ? _readyHintText.Take(2).ToList() : _readyHintText, gameObject);
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
        [SerializeField] private Transform _switch;
        [SerializeField] private string _badSwitchUpHintText;
        [SerializeField] private string _badSwitchDownHintText;
      
        #endregion
        #region PrivateVars

        private ElevatorController _elevatorController;
        private bool _isFirstTry = true;
        private bool _isSwitchInspected;
        private const int ElevatorCristalId = 3;

        private bool IsCristalPlacedInSwitch => _elevatorController.IsCristalPlacedInSwitch;
        
        #endregion

    }

}