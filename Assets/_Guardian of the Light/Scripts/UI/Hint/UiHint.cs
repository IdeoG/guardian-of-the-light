using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    [RequireComponent(typeof(InventoryEntity))]
    public class UiHint : BaseAction
    {
        [SerializeField] private HintType _hintType;
        [SerializeField] private string _hintText;
            
        private IHintController _controller;
        private InventoryEntity _entity;

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