using UnityEngine;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class HintController : MonoBehaviour, IEmptyHint, ISkipHint, ITemporaryButtonHint, IYesNoHint
    {
        private InventorySystem _inventory;
        private HintManager _manager;
        
        private void Awake()
        {
            _inventory = InventorySystem.Instance;
            _manager = FindObjectOfType<HintManager>();
        }

        public void OnEmptyExpired()
        {
            throw new System.NotImplementedException();
        }

        public void OnSkipPressed()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void OnNoPressed()
        {
            throw new System.NotImplementedException();
        }
    }
}