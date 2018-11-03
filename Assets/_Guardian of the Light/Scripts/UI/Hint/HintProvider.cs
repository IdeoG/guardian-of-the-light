using System.Collections.Generic;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class HintProvider : MonoBehaviour, IHintController,
        IEmptyHint, ISkipHint, ITemporaryButtonHint, IYesNoHint,
        IMultipleChoiceHint
    {
        private InputSystem _input;
        private HintManager _manager;
        private GameObject _gameLogicObject;
        private InventoryEntity _entity;
        private InventorySystem _inventory;
        private ThirdPersonUserControl _playerControls;
        
        public void OnShowHintButtonPressed(HintType type, string text, InventoryEntity entity)
        {
            _entity = entity;
            _manager.ShowHint(type, text);
            _input.IsUiActive = true;
            _playerControls.LockInput = true;
        }

        public void OnShowHintButtonPressed(HintType type, List<string> texts, GameObject gameLogicObject)
        {
            _gameLogicObject = gameLogicObject;
            _manager.ShowHint(type, texts);
            _input.IsUiActive = true;
            _playerControls.LockInput = true;
        }

        public void OnEmptyExpired()
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
        }

        public void OnSkipPressed()
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
        }

        public void OnConfirmPressed(int position)
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
            
            _gameLogicObject.GetComponent<IUiHint>().OnItemChosen(position);
        }

        public void OnHintExpired()
        {
            throw new System.NotImplementedException();
        }

        public void OnButtonPressed()
        {
            throw new System.NotImplementedException();
        }

        public void OnYesPressed()
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
            
            if (_entity.Id != 0) 
                _inventory.GetItemById(_entity.Id).IsTook = true;
            _entity.gameObject.GetComponent<IUiHint>().DestroyItem();
        }

        public void OnNoPressed()
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
        }

        public void OnExitPressed()
        {
            _input.IsUiActive = false;
            _playerControls.LockInput = false;
        }
        
        private void Start()
        {
            _inventory = InventorySystem.Instance;
            _input = InputSystem.Instance;
            _manager = FindObjectOfType<HintManager>();
            _playerControls = GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>();
        }
    }
}