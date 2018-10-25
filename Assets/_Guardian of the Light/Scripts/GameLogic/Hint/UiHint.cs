using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.UI.Hint;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Hint
{
    [RequireComponent(typeof(InventoryEntity))]
    public class UiHint : BaseAction, IUiHint
    {
        [SerializeField] private HintType _hintType;
        [TextArea] [SerializeField] private string _hintText;
            
        private IHintController _controller;
        private InventoryEntity _entity;

        public virtual void DestroyItem()
        {
            Destroy(gameObject);
        }
        
        protected override void OnKeyActionPressedDown()
        {
            _controller.OnShowHintButtonPressed(_hintType, _hintText, _entity);
        }

        private void Awake()
        {
            _entity = GetComponent<InventoryEntity>();
            _controller = FindObjectOfType<HintController>().GetComponent<HintController>();
        }
    }
}
