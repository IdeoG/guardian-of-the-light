using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class HintController : MonoBehaviour, IHintController,
        IEmptyHint, ISkipHint, ITemporaryButtonHint, IYesNoHint
    {
        private HintManager _manager;
        private InventoryEntity _entity;
        private InventorySystem _inventory;
        private ThirdPersonUserControl _playerControls;
        
        private void Start()
        {
            _inventory = InventorySystem.Instance;
            _manager = FindObjectOfType<HintManager>();
            _playerControls = GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>();
        }

        public void OnEmptyExpired()
        {
            _playerControls.LockInput = false;
        }

        public void OnSkipPressed()
        {
            _playerControls.LockInput = false;
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
            _playerControls.LockInput = false;
            _inventory.GetItemById(_entity.Id).IsTook = true;
            Destroy(_entity.gameObject);
        }

        public void OnNoPressed()
        {
            _playerControls.LockInput = false;
        }

        public void OnShowHintButtonPressed(HintType type, string text, InventoryEntity entity)
        {
            _entity = entity;
            _manager.ShowHint(type, text);
            _playerControls.LockInput = true;
        }
    }
}