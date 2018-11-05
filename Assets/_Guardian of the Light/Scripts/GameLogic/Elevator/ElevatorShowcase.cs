using UnityEngine;
using _Guardian_of_the_Light.Scripts.Base.Inventory;
using _Guardian_of_the_Light.Scripts.GameLogic.Hint;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Elevator
{
    public class ElevatorShowcase : UiHint, IInventoryUseAction
    {
        [SerializeField] private GameObject _gear;

        private const int ElevatorGearId = 1;
        
        public bool OnInventoryUseAction(int itemId)
        {
            if (ElevatorGearId != itemId)
                return false;
            
            _gear.SetActive(true);
            GetComponentInParent<ElevatorController>().IsGearPlacedInMechanism = true;
            GetComponentInParent<Animator>().SetBool("IsMechanismBroken", false);
            
            OnTriggerExit(null);
            GetComponent<BoxCollider>().enabled = false;
            
            return true;
        }
    }
}