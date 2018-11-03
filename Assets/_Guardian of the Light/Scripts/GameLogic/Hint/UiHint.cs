using System;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.UI.Hint;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Hint
{
    [RequireComponent(typeof(InventoryEntity))]
    public class UiHint : BaseAction, IUiHint
    {
        [SerializeField] protected HintType _hintType;
        [TextArea] [SerializeField] protected string _hintText;
            
        protected IHintController _controller;
        protected InventoryEntity _entity;

        public virtual void DestroyItem()
        {
            Destroy(gameObject);
        }

        public virtual void OnItemChosen(int position)
        {
            throw new NotImplementedException();
        }
        
        protected override void OnKeyActionPressedDown()
        {
            _controller.OnShowHintButtonPressed(_hintType, _hintText, _entity);
        }

        protected virtual void Awake()
        {
            _entity = GetComponent<InventoryEntity>();
            _controller = FindObjectOfType<HintProvider>().GetComponent<HintProvider>();          
        }
    }
}
